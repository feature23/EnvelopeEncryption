using Microsoft.Extensions.Options;

namespace F23.EnvelopeEncryption.Tests;

public class KeyVaultContentKeyEncryptionServiceTests
{
    [Fact]
    public void CreateAzureCredentialOptions_WhenExcludeManagedIdentityCredential_ReturnsCorrectOptions()
    {
        // Arrange
        var options = Options.Create<KeyVaultEnvelopeEncryptionOptions>(new()
        {
            ExcludeManagedIdentityCredential = true
        });
        var svc = new KeyVaultContentKeyEncryptionService(null!, null!, options);

        // Act
        var result = svc.CreateAzureCredentialOptions();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.ExcludeManagedIdentityCredential);
    }
    
    [Fact]
    public void CreateAzureCredentialOptions_WhenNotExcludeManagedIdentityCredential_ReturnsNull()
    {
        // Arrange
        var options = Options.Create<KeyVaultEnvelopeEncryptionOptions>(new()
        {
            ExcludeManagedIdentityCredential = false
        });
        var svc = new KeyVaultContentKeyEncryptionService(null!, null!, options);

        // Act
        var result = svc.CreateAzureCredentialOptions();

        // Assert
        Assert.Null(result);
    }
}