namespace F23.EnvelopeEncryption;

/// <summary>
/// A service for encrypting Content Encryption Keys (CEKs).
/// Warning: This is a low-level API. You probably want to be using the convenience methods on
/// <see cref="IEnvelopeEncryptionService"/> instead.
/// </summary>
public interface IContentKeyEncryptionService
{
    /// <summary>
    /// Encrypts the Content Encryption Key (CEK) so that it can be safely stored
    /// alongside the encrypted content values.
    /// </summary>
    /// <param name="key">The CEK to encrypt.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>Returns a record with the encrypted CEK and any information needed during decryption.</returns>
    Task<EncryptedKey> EncryptContentEncryptionKeyAsync(byte[] key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Encrypts a list of Content Encryption Keys (CEKs) so that they can be safely stored
    /// alongside encrypted content values. Order of encrypted key output matches order
    /// of cleartext input.
    /// </summary>
    /// <param name="keys">The CEKs to encrypt.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>Returns a list of records with the encrypted CEKs and any information needed during decryption.</returns>
    Task<IList<EncryptedKey>> EncryptContentEncryptionKeysAsync(IList<byte[]> keys,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Decrypts the encrypted Content Encryption Key (CEK), producing a byte-array
    /// key that can be used in AES encryption/decryption operations.
    /// </summary>
    /// <param name="key">The encrypted CEK.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>Returns the decrypted CEK.</returns>
    Task<byte[]> DecryptContentEncryptionKeyAsync(EncryptedKey key, CancellationToken cancellationToken = default);
}