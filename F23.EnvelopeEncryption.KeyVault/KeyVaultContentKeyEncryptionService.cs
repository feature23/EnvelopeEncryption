using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using Microsoft.Extensions.Options;

namespace F23.EnvelopeEncryption.KeyVault;

/// <summary>
/// An implementation of <see cref="IContentKeyEncryptionService"/> that uses Key Vault for encrypting Content
/// Encryption Keys (CEKs) with an asymmetric key.
/// </summary>
public class KeyVaultContentKeyEncryptionService : IContentKeyEncryptionService
{
    private readonly KeyClient _keyClient;
    private readonly CryptographyClientFactory _cryptographyClientFactory;
    private readonly KeyVaultEnvelopeEncryptionOptions _options;

    /// <summary>
    /// Creates a new KeyVaultEnvelopeEncryptionService instance.
    /// </summary>
    /// <param name="keyClient">The Key Vault Key client.</param>
    /// <param name="cryptographyClientFactory">A Cryptography Client Factory instance.</param>
    /// <param name="options">Options for this service.</param>
    public KeyVaultContentKeyEncryptionService(
        KeyClient keyClient,
        CryptographyClientFactory cryptographyClientFactory, 
        IOptions<KeyVaultEnvelopeEncryptionOptions> options)
    {
        _keyClient = keyClient;
        _cryptographyClientFactory = cryptographyClientFactory;
        _options = options.Value;
    }

    /// <summary>
    /// Encrypts the given plaintext <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The plaintext Content Encryption Key (CEK) to encrypt.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>Returns the encrypted key.</returns>
    public async Task<EncryptedKey> EncryptContentEncryptionKeyAsync(byte[] key, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(_options.KeyEncryptionKeyName))
        {
            throw new InvalidOperationException($"Missing KeyEncryptionKeyName options value in {KeyVaultEnvelopeEncryptionOptions.Options}");
        }
        
        var kek = await _keyClient.GetKeyAsync(_options.KeyEncryptionKeyName, _options.KeyEncryptionKeyVersion, 
            cancellationToken);

        var cryptoClient = _cryptographyClientFactory.CreateFromKeyUri(kek.Value.Id, CreateAzureCredentialOptions());

        // TODO: support specifying algorithm in options
        var result = await cryptoClient.EncryptAsync(EncryptParameters.RsaOaep256Parameters(key), cancellationToken);

        return new EncryptedKey(result.KeyId, result.Ciphertext, result.Algorithm.ToString());
    }

    /// <summary>
    /// Decrypts the given encrypted <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The encrypted Content Encryption Key (CEK) to decrypt.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>Returns the decrypted key.</returns>
    public async Task<byte[]> DecryptContentEncryptionKeyAsync(EncryptedKey key, CancellationToken cancellationToken = default)
    {
        var cryptoClient = _cryptographyClientFactory.CreateFromKeyUri(key.KeyId, CreateAzureCredentialOptions());
        
        // TODO: support determining algorithm from key.Algorithm value
        var result = await cryptoClient.DecryptAsync(DecryptParameters.RsaOaep256Parameters(key.Key), cancellationToken);

        return result.Plaintext;
    }

    internal DefaultAzureCredentialOptions? CreateAzureCredentialOptions()
    {
        if (_options.ExcludeManagedIdentityCredential)
        {
            return new DefaultAzureCredentialOptions
            {
                ExcludeManagedIdentityCredential = true
            };
        }

        return null;
    }
}
