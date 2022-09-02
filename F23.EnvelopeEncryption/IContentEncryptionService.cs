namespace F23.EnvelopeEncryption;

/// <summary>
/// A service for content-level cryptographic operations.
/// Warning: This is a low-level API. You probably want to be using the convenience methods on
/// <see cref="IEnvelopeEncryptionService"/> instead.
/// </summary>
public interface IContentEncryptionService
{
    /// <summary>
    /// Creates a new plaintext (unencrypted) Content Encryption Key (CEK).
    /// A new CEK should be generated for each content value.
    /// Warning: This plaintext CEK is unencrypted, and must be encrypted via the
    /// <see cref="IEnvelopeEncryptionService"/> before persisting to storage.
    /// </summary>
    /// <returns>Returns a new non-null byte array containing the new CEK.</returns>
    byte[] CreateContentEncryptionKey();

    /// <summary>
    /// Encrypts the provided content <paramref name="value"/> string using the given plaintext <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The plaintext Content Encryption Key (CEK).</param>
    /// <param name="value">The plaintext string value to encrypt.</param>
    /// <returns>Returns a new EncryptedValue instance.</returns>
    EncryptedValue EncryptContentString(byte[] key, string value);

    /// <summary>
    /// Decrypts the provided encrypted <paramref name="value"/> using the given plaintext <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The plaintext Content Encryption Key (CEK).</param>
    /// <param name="value">The encrypted value to decrypt.</param>
    /// <returns>Returns the decrypted content string.</returns>
    string DecryptContentString(byte[] key, EncryptedValue value);
}