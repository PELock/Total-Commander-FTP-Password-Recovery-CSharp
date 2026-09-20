# Total Commander FTP Password Recovery — C# / .NET

Offline decoder for Total Commander `wcx_ftp.ini` FTP password hex fields. Not a Web API client.

```bash
dotnet add package PELock.TotalCommanderFtpPasswordRecovery
```

```csharp
using PELock.TotalCommanderFtpPasswordRecovery;

var decoder = new TotalCommanderPasswordDecoder();
var plain = decoder.DecryptPasswordString("00112233445566778899aabbccddeeff");
```

Whitespace and hex case are ignored. Apache-2.0. Copyright Bartosz Wójcik / PELock.
