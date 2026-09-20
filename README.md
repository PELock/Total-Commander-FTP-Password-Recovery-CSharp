# Total Commander FTP password recovery (C#)

[Total Commander](https://www.ghisler.com/) (formerly known as ~~Windows Commander~~) is a classic file manager for Windows, Windows CE, Windows Phone, and now also Android.

Total Commander has a built-in FTP/FXP client and it keeps the FTP logins and encrypted passwords in **wcx_ftp.ini** configuration file.

![Total Commander FTP Password Recovery Tool](https://www.pelock.com/img/en/products/total-commander-ftp-password-recovery/total-commander-ftp-password-recovery.png "Total Commander FTP Password Recovery Tool")

I have [reverse engineered](https://www.pelock.com/services) and recreated the password decoding algorithm years ago.

It was made available by me to another [FlashFXP](https://www.flashfxp.com/) software to import FTP connection profiles from Total Commander.

I give you source codes for both the original assembly decoding algorithm and a C# implementation of this algorithm.

## Total Commander Online Password Decoder

You can either use one of the provided source codes or use my own online implementation to make things faster:

[https://www.pelock.com/products/total-commander-ftp-password-recovery](https://www.pelock.com/products/total-commander-ftp-password-recovery)

## Installation

The preferred way to install the library is via NuGet.

```bash
dotnet add package PELock.TotalCommanderFtpPasswordRecovery
```

Or add `<PackageReference Include="PELock.TotalCommanderFtpPasswordRecovery" Version="1.0.0" />` to your `.csproj`.

Package listing: https://www.nuget.org/packages/PELock.TotalCommanderFtpPasswordRecovery

## Usage examples

### Example — `basic-usage.cs`

```csharp
/******************************************************************************
 * Total Commander FTP Password Recovery usage example.
 *
 * Version        : v1.0.1
 * Language       : C#
 * Author         : Bartosz Wójcik
 * Web page       : https://www.pelock.com
 *
 *****************************************************************************/

using PELock.TotalCommanderFtpPasswordRecovery;

const string cipherHex = "00112233445566778899aabbccddeeff";
var decoder = new TotalCommanderPasswordDecoder();
var plain = decoder.DecryptPassword(cipherHex);

if (plain is null)
{
    Console.Error.WriteLine("Decode failed.");
    return 1;
}

Console.WriteLine(System.Text.Encoding.Latin1.GetString(plain));
Console.WriteLine("hex: " + Convert.ToHexString(plain).ToLowerInvariant());
return 0;
```

See the `examples/` directory in this repository for complete samples.


## References

- [Total Commander FTP Password Recovery (article / online tool)](https://www.pelock.com/products/total-commander-ftp-password-recovery)
- [Source code recovery / reverse engineering services](https://www.pelock.com/services)

## Author

Bartosz Wójcik

- Visit my site at — [https://www.pelock.com](https://www.pelock.com)
- X — [https://x.com/PELock](https://x.com/PELock)
- GitHub — [https://github.com/PELock](https://github.com/PELock)
