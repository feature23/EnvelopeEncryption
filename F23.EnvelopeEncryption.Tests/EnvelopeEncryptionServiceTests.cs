namespace F23.EnvelopeEncryption.Tests;

public class EnvelopeEncryptionServiceTests
{
    [Fact]
    public async Task EncryptionRoundTripTest()
    {
        var aesCbc = new AesCbcContentEncryptionService();
        var mockKeyService = new MockContentKeyEncryptionService();

        var envelopeService = new EnvelopeEncryptionService(aesCbc, mockKeyService);

        const string input = "This is a test string";

        var encrypted = await envelopeService.EncryptContentStringWithNewKeyAsync(input);

        var decrypted = await envelopeService.DecryptContentStringAsync(encrypted);
        
        Assert.Equal(input, decrypted);
    }
}