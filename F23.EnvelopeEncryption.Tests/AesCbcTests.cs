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
}