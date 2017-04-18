# GoogleDomainsDdnsService
C# Windows Service (**.NET Framework 4.6**) to update your Google Domains Dynamic DNS for your Google hosted domains

Build it yourself or grab the binaries

## How to Use (binaries)

* Download the contents of the build folder to a local folder
* Add all the hostnames and credentials you have setup to utilize DynamicDNS on domains.google.com in the GoogleDomainsDdnsSvc.config file
* Open an administrator command prompt and run GoogleDomainsDdnsSvc.exe --install
* Start your Service
* Verify in the Application Event log that something happened and enjoy

## How to Use (when building)

* Open solution file in Visual Studio or folder/csproj in VS Code.
* Rename App.config.sample to App.config
* Add all the hostnames and credentials you have setup to utilize DynamicDNS on domains.google.com
* Build and publish to wherever you like
* Open an administrator command prompt and run GoogleDomainsDdnsSvc.exe --install
* Start your Service
* Verify in the Application Event log that something happened and enjoy