# NullFX Windows Service Editor

The NullFX Windows Service Editor is a utility that allows applications to be installed or uninstalled as windows services.

Applications it registers need to implement windows service apis, either the [standard Win32](https://docs.microsoft.com/en-us/windows/win32/api/winsvc/nc-winsvc-lpservice_main_functionw) functions or the .net [ServiceBase](https://docs.microsoft.com/en-us/dotnet/api/system.serviceprocess.servicebase?view=dotnet-plat-ext-5.0) base class.  Executables containing these implementations will be able to register services using this utility.

## Installing An Application as a service

To install an application as a service simply:

1. Provide the applications Service name
2. Populate the applications Display name
3. Specify the file path to the executable
4. If the service requires a Log On as account, specify the credentials to the user who has been assigned to the Log On As service group
5. And provider the users password
6. Click Install

![](img/install.png)
![](img/installed.png)


## Uninstalling An Application as a service

NOTE: This application has the ability to uninstall **_any and all windows services_** including those required by the operating system.

Because of this, it is possible, plausable, and most likely that you WILL do **_irreparable_** and catastrophic damage to your operating system for which you alone are responsible for. 

Use of this 

1. Select the service you wish to uninstall
2. Click Uninstall
3. Confirm you wish to uninstall

![](img/select_uninstall.png)
![](img/uninstall.png)
![](img/confirm_uninstall.png)
![](img/uninstalled.png)


