namespace F23.EnvelopeEncryption;

/// <summary>
/// A default implementation of <see cref="IEnvelopeEncryptionService"/> that uses the configured
/// <see cref="IContentEncryptionService"/> and <see cref="IContentKeyEncryptionService"/>.
/// </summary>
public class EnvelopeEncryptionService : IEnvelopeEncryptionService
{
    private readonly IContentEncryptionService _contentEncryptionService;
    private readonly IContentKeyEncryptionService _contentKeyEncryptionService;

    /// <summary>
    /// Creates a new EnvelopeEncryptionService instance.
    /// </summary>
    /// <param name="contentEncryptionService">A content encryption service instance.</param>
    /// <param name="contentKeyEncryptionService">A content key encryption service instance.</param>
    public EnvelopeEncryptionService(
        IContentEncryptionService contentEncryptionService, 
        IContentKeyEncryptionService contentKeyEncryptionService)
    {
        _contentEncryptionService = contentEncryptionService;
        _contentKeyEncryptionService = contentKeyEncryptionService;
    }

    /// <summary>
    /// Encrypts the given content string <paramref name="value"/> with a newly-generated Content Encryption Key (CEK).
    /// </summary>
    /// <param name="value">The unencrypted content string to encrypt.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>Returns the KEK-encrypted newly-generated CEK and CEK-encrypted value.</returns>
    public async Task<EncryptedKeyAndValue> EncryptContentStringWithNewKeyAsync(string value, 
        CancellationToken cancellationToken = default)
    {
        var key = _contentEncryptionService.CreateContentEncryptionKey();

        var encryptedKey = await _contentKeyEncryptionService.EncryptContentEncryptionKeyAsync(key, cancellationToken);

        var encryptedValue = _contentEncryptionService.EncryptContentString(key, value);

        return new EncryptedKeyAndValue(encryptedKey, encryptedValue);
    }

    /// <summary>
    /// Decrypts the given Content Encryption Key (CEK) and value pair.
    /// </summary>
    /// <param name="keyAndValue">The KEK-encrypted CEK and CEK-encrypted value pair.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>Returns the unencrypted plaintext string.</returns>
    public async Task<string> DecryptContentStringAsync(EncryptedKeyAndValue keyAndValue, 
        CancellationToken cancellationToken = default)
    {
        var key = await _contentKeyEncryptionService.DecryptContentEncryptionKeyAsync(keyAndValue.Key, cancellationToken);

        return _contentEncryptionService.DecryptContentString(key, keyAndValue.Value);
    }
}