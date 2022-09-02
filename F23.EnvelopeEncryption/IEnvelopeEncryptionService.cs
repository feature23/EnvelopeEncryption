namespace F23.EnvelopeEncryption;

/// <summary>
/// A service for working with the Envelope Encryption pattern.
/// </summary>
public interface IEnvelopeEncryptionService
{
    /// <summary>
    /// Encrypts the given content string <paramref name="value"/> with a newly generated Content Encryption Key (CEK),
    /// and returns the encrypted CEK and the encrypted value.
    /// </summary>
    /// <param name="value">The content string to encrypt.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>Returns the encrypted CEK and value pair.</returns>
    Task<EncryptedKeyAndValue> EncryptContentStringWithNewKeyAsync(string value,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Decrypts the given encrypted Content Encryption Key and value pair as a string.
    /// </summary>
    /// <param name="keyAndValue">The encrypted Content Encryption Key and encrypted value pair.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>Returns the decrypted content string.</returns>
    Task<string> DecryptContentStringAsync(EncryptedKeyAndValue keyAndValue,
        CancellationToken cancellationToken = default);
}