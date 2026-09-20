/******************************************************************************
 * Total Commander FTP Password Recovery
 *
 * Version        : v1.0.1
 * Language       : C#
 * Author         : Bartosz Wójcik
 * Web page       : https://www.pelock.com
 *
 *****************************************************************************/

namespace PELock.TotalCommanderFtpPasswordRecovery;

/// <summary>
/// Total Commander FTP password decoder (wcx_ftp.ini stored form).
/// Checksum validation present in Total Commander is not implemented here.
/// </summary>
public sealed class TotalCommanderPasswordDecoder
{
    private uint _randomSeed;

    public static byte[]? HexStringToByteArray(string str)
    {
        var lowered = str.ToLowerInvariant();
        var len = lowered.Length;
        if (len == 0 || (len & 1) != 0)
            return null;

        var result = new byte[len / 2];
        for (var i = 0; i < len; i += 2)
        {
            var hi = HexNibble(lowered[i]);
            var lo = HexNibble(lowered[i + 1]);
            if (hi < 0 || lo < 0)
                return null;
            result[i / 2] = (byte)((hi << 4) | lo);
        }

        return result;
    }

    public void SeedPrng(int seed) => _randomSeed = unchecked((uint)seed);

    private int NextRandMax(int nMax)
    {
        unchecked
        {
            _randomSeed = _randomSeed * 0x8088405u + 1u;
            return (int)(((ulong)_randomSeed * (ulong)nMax) >> 32);
        }
    }

    public static int Rol8(int value, int counter)
    {
        var b = value & 0xFF;
        var c = counter & 7;
        return ((b << c) | (b >> (8 - c))) & 0xFF;
    }

    /// <summary>Decrypt hex ciphertext. Returns UTF-8 plaintext bytes, or null on invalid input.</summary>
    public byte[]? DecryptPassword(string passwordHex)
    {
        var normalized = string.Concat(passwordHex.Where(static c => !char.IsWhiteSpace(c)));
        var bytes = HexStringToByteArray(normalized.ToLowerInvariant());
        if (bytes is null)
            return null;

        var passwordLength = bytes.Length;
        if (passwordLength <= 4)
            return null;

        passwordLength -= 4;
        var buf = (byte[])bytes.Clone();

        SeedPrng(849521);
        for (var i = 0; i < passwordLength; i++)
            buf[i] = (byte)Rol8(buf[i], NextRandMax(8));

        SeedPrng(12345);
        for (var i = 0; i < 256; i++)
        {
            var x = NextRandMax(passwordLength);
            var y = NextRandMax(passwordLength);
            (buf[x], buf[y]) = (buf[y], buf[x]);
        }

        SeedPrng(42340);
        for (var i = 0; i < passwordLength; i++)
            buf[i] ^= (byte)NextRandMax(256);

        SeedPrng(54321);
        for (var i = 0; i < passwordLength; i++)
            buf[i] = (byte)((buf[i] - NextRandMax(256)) & 0xFF);

        var output = new byte[passwordLength];
        Array.Copy(buf, output, passwordLength);
        return output;
    }

    /// <summary>Decrypt hex ciphertext to a Latin-1/byte string (matches PHP chr() output).</summary>
    public string? DecryptPasswordString(string passwordHex)
    {
        var bytes = DecryptPassword(passwordHex);
        return bytes is null ? null : System.Text.Encoding.Latin1.GetString(bytes);
    }

    private static int HexNibble(char c)
    {
        if (c is >= '0' and <= '9') return c - '0';
        if (c is >= 'a' and <= 'f') return c - 'a' + 10;
        return -1;
    }
}
