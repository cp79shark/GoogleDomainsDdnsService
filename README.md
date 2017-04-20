# GoogleDomainsDdnsService
C# Windows Service (**.NET Framework 4.6**) to update your Google Domains Dynamic DNS subdomains for your Google hosted domains

Build it yourself or grab the binaries

## What is this for?

If you want to host a service (web site, etc) on your local connection, but your ISP issues you a dynamic IP address, you'll need
a way to update your DNS entry for the service so that the outside world knows that XYZ.blah.com has a new IP address. 

That's where this comes in, but only for domains hosted by Google.

If you're using Google Domains as your domain registrar, you can specify a Dynamic DNS subdomain. Log in to <https://domains.google.com/>
and click on DNS for your domain. There will be a **Synthetic records** section. Change the dropdown to **Dynamic DNS** and enter a 
subdomain. [ XYZ ] .blah.com. Click Add.

You know have a Dynamic DNS subdomain. Expand the Dynamic DNS section for your created subdomain and click **View Credentials**. 
You'll need these to configure GoogleDomainsDdnsSvc.

## Configuration File Overview
The GoogleDomainsDdnsSvc.exe.config defines what hosts and how often to update their Dynamic DNS entries. 
The default interval is every 24 hours an update will occur. There are five fields that make up a <domains> collection entry.

* hostname: The host entry (xyz.blah.com) you wish to update
* username: The generated username for the Dynamic DNS host entry from **Dynamic DNS** / (your subdomain) / **View Credentials** on the Google Domains DNS manager
* password: The generated password for the Dynamic DNS host entry from **Dynamic DNS** / (your subdomain) / **View Credentials** on the Google Domains DNS manager
* longDelay: Time in milliseconds between each Dynamic DNS update (Optional, default 86400000 or 24 hours)
* shortDelay: Time in milliseconds between each Dynamic DNS update (Optional, default 600000 or 10 minutes)

    <googleDomains>
        <domains>
            <!-- good idea to clear -->
            <clear />
            <!-- get your ddns credentials for each hostname from your account at domains.google.com -->
            <add hostname="blah.com" username="ABC123" password="ABC123" />
        </domains>
    </googleDomains>

## How to Use (binaries)

* Download the contents of the build folder to a local folder (e.g. C:\ProgramFiles\GoogleDomainsDdnsSvc\)
* Add all the hostnames and credentials you have setup to utilize DynamicDNS on domains.google.com in the GoogleDomainsDdnsSvc.exe.config file
* Open an administrator command prompt and run GoogleDomainsDdnsSvc.exe --install
* Start your Service
* Verify in the Application Event log that something happened and enjoy

## How to Use (when building)

* Open solution file in Visual Studio or folder/csproj in VS Code.
* Rename App.config.sample to App.config
* Add all the hostnames and credentials you have setup to utilize DynamicDNS on domains.google.com
* Build and publish to wherever you like (e.g. C:\ProgramFiles\GoogleDomainsDdnsSvc\)
* Open an administrator command prompt and run GoogleDomainsDdnsSvc.exe --install
* Start your Service
* Verify in the Application Event log that something happened and enjoy