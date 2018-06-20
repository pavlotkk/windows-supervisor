# windows-supervisor
Supervisor based on Windows Service for running background tasks. 
For example Python [Flask](http://flask.pocoo.org/) server application.


## Config
`App.config` has next structure:
```xml
  <appSettings>
    <add key="appname" value="WinSupervisor" />
    <add key="filename" value="python" />
    <add key="args" value="--version"/>
    <add key="logger.console.enable" value="true"/>
    <add key="logger.windows.event.enable" value="true"/>
    <add key="autorestart" value="true"/>
    <add key="restarttimeout" value="3000"/>
  </appSettings>
```
- `appname`: Application name;
- `filename`: Executable filename;
- `args`: Arguments to start with;
- `logger.console.enable`: Add Console logger. Useful for debugging;
- `logger.windows.event.enable`: Add Windows Event Logger;
- `autorestart`: Restart executable if it failed. Have no sense to set it **false**
- `restarttimeout`: Restart failed execution after *x* milliseconds.


## How to Install using Visual Studio:
1. Build project
2. Run as Administrator **Developer Command Prompt** under Start menu
3. Run `installutil.exe WinSupervisor.exe`

Or
2. `sc create <ServiceName> binPath= "<Service.exe>"`
For example:
`sc create WinSupervisor binPath= "WinSupervisor.exe"`

## How to Uninstall:
1. Run as Administrator **Developer Command Prompt** under Start menu
2. Run `installutil.exe /u WinSupervisor.exe`

Or:
1. --
2. `sc delete <ServiceName>`
For example:
`sc delete WinSupervisor`

Mode details about install/uninstall [https://docs.microsoft.com/en-us/dotnet/framework/windows-services/how-to-install-and-uninstall-services](https://docs.microsoft.com/en-us/dotnet/framework/windows-services/how-to-install-and-uninstall-services)


## Start/Stop installed service
Start: `net start <ServiceName>`

Stop: `net stop <ServiceName>`


## Tips and tricks
Build C# project via cmd:
`<Path to .NET Framework>\MSBuild.exe <Path>\<Project>.sln /t:Rebuild /p:Configuration=Release /p:Platform="any cpu"`

For example:
`c:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe windows-supervisor\WindowsSupervisor\WinSupervisor.sln /t:Rebuild /p:Configuration=Release /p:Platform="any cpu"`