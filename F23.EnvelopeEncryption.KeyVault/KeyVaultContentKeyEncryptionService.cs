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
    private readonly KeyVaultEnvelopeEncryptionOptions _options;

    /// <summary>
    /// Creates a new KeyVaultEnvelopeEncryptionService instance.
    /// </summary>
    /// <param name="keyClient">The Key Vault Key client.</param>
    /// <param name="options">Options for this service.</param>
    public KeyVaultContentKeyEncryptionService(
        KeyClient keyClient,
        IOptions<KeyVaultEnvelopeEncryptionOptions> options)
    {
        _keyClient = keyClient;
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
        
        var cryptoClient = _keyClient.GetCryptographyClient(_options.KeyEncryptionKeyName, _options.KeyEncryptionKeyVersion);

        // TODO: support specifying algorithm in options
        var result = await cryptoClient.EncryptAsync(EncryptParameters.RsaOaep256Parameters(key), cancellationToken);

        return new EncryptedKey(result.KeyId, result.Ciphertext, result.Algorithm.ToString());
    }

    /// <summary>
    /// Encrypts a list of Content Encryption Keys (CEKs) so that they can be safely stored
    /// alongside encrypted content values. Order of encrypted key output matches order
    /// of cleartext input.
    /// </summary>
    /// <param name="keys">The CEKs to encrypt.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>Returns a list of records with the encrypted CEKs and any information needed during decryption.</returns>
    public async Task<IList<EncryptedKey>> EncryptContentEncryptionKeysAsync(IList<byte[]> keys,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(_options.KeyEncryptionKeyName))
        {
            throw new InvalidOperationException($"Missing KeyEncryptionKeyName options value in {KeyVaultEnvelopeEncryptionOptions.Options}");
        }

        var cryptoClient = _keyClient.GetCryptographyClient(_options.KeyEncryptionKeyName, _options.KeyEncryptionKeyVersion);
        
        // TODO: support specifying algorithm in options
        var results = await keys.ToAsyncEnumerable()
            .SelectAwaitWithCancellation(async (key, ct) =>
                await cryptoClient.EncryptAsync(EncryptParameters.RsaOaep256Parameters(key), ct))
            .Select(result => new EncryptedKey(result.KeyId, result.Ciphertext, result.Algorithm.ToString()))
            .ToListAsync(cancellationToken);

        return results;
    }

    /// <summary>
    /// Decrypts the given encrypted <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The encrypted Content Encryption Key (CEK) to decrypt.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>Returns the decrypted key.</returns>
    public async Task<byte[]> DecryptContentEncryptionKeyAsync(EncryptedKey key, CancellationToken cancellationToken = default)
    {
        var keyIdentifier = new KeyVaultKeyIdentifier(new Uri(key.KeyId));
        
        var cryptoClient = _keyClient.GetCryptographyClient(keyIdentifier.Name, keyIdentifier.Version);
        
        // TODO: support determining algorithm from key.Algorithm value
        var result = await cryptoClient.DecryptAsync(DecryptParameters.RsaOaep256Parameters(key.Key), cancellationToken);

        return result.Plaintext;
    }
}
