# NanoServerApiScan

> This content is originally from: https://blogs.technet.microsoft.com/nanoserver/2016/04/27/nanoserverapiscan-exe-updated-for-tp5/

If you developed a 64-bit application, tool, or agent for Windows Server in C/C++, you can use `NanoServerApiScan.exe` to check if your app will also run on Nano Server. **Remember that Nano Server is 64-bit only and won’t run 32-bit binaries.**

`NanoServerApiScan.exe` scans a directory containing your binaries and reports an error if it finds an API that is not available in Nano Server. It even provides replacement API suggestions in many cases. `NanoServerApiScan.exe` requires .NET Framework version 4.0 or higher.

For a list of supported APIs in Nano Server, please go to: https://msdn.microsoft.com/en-us/library/mt588480(v=vs.85).aspx

## Setup

1. Download the attached exe file and copy it to a local folder
2. Open a Nano Server image and copy all the files under "C:\Windows\System32\Forwarders" to the same local folder you copied the attached exe to.
3. Download and install the Windows 10 SDK: https://developer.microsoft.com/en-US/windows/downloads/windows-10-sdk

## Syntax

```
NanoServerApiScan.exe /BinaryPath:<directory containing your binaries> /WindowsKitsPath:<Windows SDK directory>
```

- `/BinaryPath` _(Mandatory)_  
  The directory containing your binaries. NanoServerApiScan.exe will parse all sub-directories as well.
- `/WindowsKitsPath` _(Mandatory)_  
  The Windows 10 SDK contains OneCore.lib and other important files for NanoServerApiScan.exe. Use this parameter to specify the Windows 10 SDK directory.

## Example

```
NanoServerApiScan.exe /BinaryPath:e:\Temp\Test /WindowsKitsPath:"C:\Program Files (x86)\Windows Kits"
```

Sample output:

```
=== e:\Temp\Test\LrmApi.dll ===

ERRORS:

  KERNEL32.dll
    GetStartupInfoA (Proc not found)
      Please use API GetStartupInfoW as substitution.
    SetHandleCount (Proc not found)
```
