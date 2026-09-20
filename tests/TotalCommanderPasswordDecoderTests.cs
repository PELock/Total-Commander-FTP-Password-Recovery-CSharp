using PELock.TotalCommanderFtpPasswordRecovery;
using Xunit;

namespace PELock.TotalCommanderFtpPasswordRecovery.Tests;

public sealed class TotalCommanderPasswordDecoderTests
{
    private static readonly byte[] Expected = Convert.FromHexString("fdf3b350e9b8c5fbe82d478d");

    [Fact]
    public void DecryptPassword_GoldenVector()
    {
        var decoder = new TotalCommanderPasswordDecoder();
        var got = decoder.DecryptPassword("00112233445566778899aabbccddeeff");
        Assert.NotNull(got);
        Assert.Equal(Expected, got);
    }

    [Fact]
    public void DecryptPassword_IgnoresWhitespaceAndCase()
    {
        var decoder = new TotalCommanderPasswordDecoder();
        var got = decoder.DecryptPassword("00 11 22 33 44 55 66 77 88 99 AA BB CC DD EE FF");
        Assert.Equal(Expected, got);
    }

    [Fact]
    public void DecryptPassword_RejectsTooShort()
    {
        Assert.Null(new TotalCommanderPasswordDecoder().DecryptPassword("00112233"));
    }

    [Fact]
    public void DecryptPassword_RejectsInvalidHex()
    {
        Assert.Null(new TotalCommanderPasswordDecoder().DecryptPassword("00112233445566778899aabbccddeefg"));
    }

    [Fact]
    public void HexStringToByteArray()
    {
        Assert.Equal(new byte[] { 0x41, 0x42 }, TotalCommanderPasswordDecoder.HexStringToByteArray("4142"));
        Assert.Null(TotalCommanderPasswordDecoder.HexStringToByteArray("414"));
        Assert.Null(TotalCommanderPasswordDecoder.HexStringToByteArray("41ag"));
    }

    [Fact]
    public void Rol8()
    {
        Assert.Equal(0xAB, TotalCommanderPasswordDecoder.Rol8(0xAB, 0));
        Assert.Equal(0x57, TotalCommanderPasswordDecoder.Rol8(0xAB, 1));
        Assert.Equal(0xAB, TotalCommanderPasswordDecoder.Rol8(0xAB, 8));
    }
}
