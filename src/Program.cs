using System;
using System.Configuration.Install;
using System.Diagnostics;
using System.Reflection;
using System.ServiceProcess;

namespace GoogleDomainsDdnsSvc
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            //var svc = new GoogleDdns();
            //svc.InternalStart();
            //Console.ReadLine();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

            if (Environment.UserInteractive)
            {
                string parameter = string.Concat(args);
                switch (parameter)
                {
                    case "--install":
                        // register the source, JIC
                        if (!EventLog.SourceExists(GoogleDdns.EventSource)) { EventLog.CreateEventSource(GoogleDdns.EventSource, "Application"); }
                        ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
                        break;
                    case "--uninstall":
                        ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                        break;
                }
            }
            else
            {
                ServiceBase.Run(new GoogleDdns());
            }
        }

        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            EventLog.WriteEntry("Application", "Failure: " + e.ExceptionObject, EventLogEntryType.Error);
        }
    }
}
