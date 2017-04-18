# GoogleDomainsDdnsService
C# Windows Service to update your Google Domains Dynamic DNS for your Google hosted domains

## How to Use

* Open in Visual Studio or VS Code.
* Rename App.config.sample to App.config
* Add all the hostnames and credentials you have setup to utilize DynamicDNS on domains.google.com
* Build and publish to wherever you like
* Open an administrator command prompt and run GoogleDomainsDdnsSvc.exe --install
* Start your Service
* Verify in the Application Event log that something happened and enjoy