using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Diagnostics;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;

namespace GoogleDomainsDdnsSvc
{
    public partial class GoogleDdns : ServiceBase
    {
        public static readonly string EventSource = "Google Domains Dynamic DNS";

        ICollection<DomainConfigElement> domains = new List<DomainConfigElement>();

        public GoogleDdns()
        {
            ServiceName = nameof(GoogleDdns);
        }

        protected override void OnStart(string[] args)
        {
            // read in configuration
            DomainsSection domainSections = (DomainsSection)ConfigurationManager.GetSection("googleDomains");

            foreach (DomainConfigElement domain in domainSections.Domains)
            {
                if (string.IsNullOrWhiteSpace(domain.HostName) || string.IsNullOrWhiteSpace(domain.UserName) || string.IsNullOrWhiteSpace(domain.Password)) { continue; }
                if (domains.Any(d => d.HostName.ToLowerInvariant() == domain.HostName.ToLowerInvariant())) { continue; }

                domains.Add(domain);
                EventLog.WriteEntry(EventSource, "Adding domain " + domain.HostName, EventLogEntryType.Information);
                domain.Timer.Interval = 1; // ensure an initial fire
                domain.Timer.Elapsed += (s,e) => {
                    MakeRequest(domain);
                };
                domain.Timer.Start();
                EventLog.WriteEntry(EventSource, "Starting domain " + domain.HostName, EventLogEntryType.Information);
            }

            if (domains.Any() == false)
            {
                EventLog.WriteEntry(EventSource, "You must set at least one domain in the application configuration file. ", EventLogEntryType.Error);
                ExitCode = -1;
                Stop();

                return;
            }
        }

        protected override void OnStop()
        {
            foreach (DomainConfigElement domain in domains)
            {
                domain.Timer.Stop();
            }
        }

        private void MakeRequest(DomainConfigElement domain)
        {          
            domain.Timer.Interval = domain.LongDelay;
            
            try
            {
                string response = string.Empty;
                string content = string.Empty;

                using (var client = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://domains.google.com/nic/update?hostname=" + domain.HostName);
                    var byteArray = Encoding.ASCII.GetBytes(domain.UserName + ":" + domain.Password);
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    request.Headers.UserAgent.ParseAdd("Chrome/57.0.2987.133");

                    var task = client.SendAsync(request);
                    task.Wait();

                    content = task.Result.Content.ReadAsStringAsync().Result;
                    response = content.Split(' ')[0];
                }

                switch (response)
                {
                    case "good":
                    case "nochg":
                        EventLog.WriteEntry(EventSource, "Good update from domains.google.com for " + domain.HostName + ": " + content, EventLogEntryType.Information);
                        break;
                    case "911":
                        // google error, wait 10 minutes for retry
                        domain.Timer.Interval = domain.ShortDelay;
                        break;
                    default:
                        // some other issue
                        EventLog.WriteEntry(EventSource, "Failure update from domains.google.com for " + domain.HostName + ": " + content, EventLogEntryType.Error);
                        break;
                }
            }
            catch (Exception ex)
            {
                domain.Timer.Interval = domain.ShortDelay;
                EventLog.WriteEntry(EventSource, "Failure update trying to contact domains.google.com for " + domain.HostName + ": " + ex.StackTrace.ToString(), EventLogEntryType.Error);
            }
        }
    }

    [RunInstaller(true)]
    public class SvcInstaller : Installer
    {
        public SvcInstaller()
        {
            var pInstaller = new ServiceProcessInstaller()
            {
                Account = ServiceAccount.LocalService
            };

            var serviceInstaller = new ServiceInstaller()
            {
                DisplayName = "Dynamic DNS Updater for Google Domains",
                Description = "Will contact domains.google.com and update the specified DNS record(s).",
                StartType = ServiceStartMode.Automatic,
                ServiceName = nameof(GoogleDdns)
            };

            Installers.Add(pInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
