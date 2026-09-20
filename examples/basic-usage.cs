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
