# nano-api-scan

A .NET Core tool to detect any Win32 APIs that are not available on Windows Nano Server.

## Usage

```
nano-api-scan <path-to-native-binary>
```

## Example

```
nano-api-scan path/to/libSkiaSharp.dll
```

Sample output:

```
=== libSkiaSharp.dll ===

ERRORS:

  FONTSUB.dll
    CreateFontPackage (Proc not found)
```

## Credits

This library uses the tool `NanoServerApiScan` from https://blogs.technet.microsoft.com/nanoserver/2016/04/27/nanoserverapiscan-exe-updated-for-tp5/

See the [externals/README.md](externals/README.md) for more information.
