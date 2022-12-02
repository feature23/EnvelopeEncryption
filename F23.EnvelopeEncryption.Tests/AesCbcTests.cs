using System.Text;

namespace F23.EnvelopeEncryption.Tests;

public class AesCbcTests
{
    [Fact]
    public void CreateContentEncryptionKey_ShouldCreateRandomKey()
    {
        var aesCbc = new AesCbcContentEncryptionService();

        var key = aesCbc.CreateContentEncryptionKey();
        
        Assert.Equal(32, key.Length);
        
        // ensure not all zeroes, although theoretically is remotely possible from random number generator
        Assert.False(key.All(i => i == 0)); 
    }

    [Fact]
    public void EncryptionRoundTripTest()
    {
        var aesCbc = new AesCbcContentEncryptionService();

        var key = aesCbc.CreateContentEncryptionKey();

        const string input = "This is a test string";

        var encrypted = aesCbc.EncryptContentString(key, input);

        var decrypted = aesCbc.DecryptContentString(key, encrypted);
        
        Assert.Equal(input, decrypted);
    }
    
    [Fact]
    public void EncryptionRoundTripTest_ByteArray()
    {
        var aesCbc = new AesCbcContentEncryptionService();

        var key = aesCbc.CreateContentEncryptionKey();

        const string input = "This is a test string";
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        
        var encrypted = aesCbc.EncryptContent(key, inputBytes);

        var decrypted = aesCbc.DecryptContent(key, encrypted);
        
        Assert.Equal(inputBytes, decrypted);
    }
    
    [Fact]
    public async Task EncryptionRoundTripTest_Stream()
    {
        var aesCbc = new AesCbcContentEncryptionService();

        var key = aesCbc.CreateContentEncryptionKey();

        const string input = "This is a test string";
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        using var inputStream = new MemoryStream(inputBytes);
        using var encryptedOutputStream = new MemoryStream();
        using var decryptedOutputStream = new MemoryStream();

        var iv = await aesCbc.EncryptContent(key, inputStream, encryptedOutputStream);

        encryptedOutputStream.Position = 0;

        await aesCbc.DecryptContent(key, iv, encryptedOutputStream, decryptedOutputStream);
        
        Assert.Equal(inputBytes, decryptedOutputStream.ToArray());
    }
}