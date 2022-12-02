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
    /// <returns>Returns a new <see cref="EncryptedValue"/> instance.</returns>
    EncryptedValue EncryptContentString(byte[] key, string value);

    /// <summary>
    /// Encrypts the provided binary content <paramref name="value" /> using the given plaintext <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The plaintext Content Encryption Key (CEK).</param>
    /// <param name="value">The plaintext byte array value to encrypt.</param>
    /// <returns>Returns a new <see cref="EncryptedValue"/> instance.</returns>
    EncryptedValue EncryptContent(byte[] key, byte[] value);

    /// <summary>
    /// Encrypts the provided plaintext <paramref name="inputStream"/> to the provided <paramref name="outputStream"/>.
    /// </summary>
    /// <param name="key">The plaintext Content Encryption Key (CEK).</param>
    /// <param name="inputStream">The <see cref="Stream"/> representing the plaintext data to be encrypted.</param>
    /// <param name="outputStream">The <see cref="Stream"/> where the encrypted content will be written.</param>
    /// <returns>Returns a task that represents the asynchronous decryption operation.</returns>
    Task<InitializationVector> EncryptContent(byte[] key, Stream inputStream, Stream outputStream);
    
    /// <summary>
    /// Decrypts the provided encrypted <paramref name="value"/> using the given plaintext <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The plaintext Content Encryption Key (CEK).</param>
    /// <param name="value">The <see cref="EncryptedValue"/> to decrypt.</param>
    /// <returns>Returns the decrypted content string.</returns>
    string DecryptContentString(byte[] key, EncryptedValue value);

    /// <summary>
    /// Decrypts the provided encrypted <paramref name="value"/> using the given plaintext <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The plaintext Content Encryption Key (CEK).</param>
    /// <param name="value">The <see cref="EncryptedValue"/> to decrypt.</param>
    /// <returns>Returns the decrypted content byte array.</returns>
    byte[] DecryptContent(byte[] key, EncryptedValue value);

    /// <summary>
    /// Decrypts the provided encrypted <paramref name="inputStream"/> to the provided <paramref name="outputStream"/>
    /// using the given plaintext <paramref name="key"/> and <paramref name="iv"/>.
    /// </summary>
    /// <param name="key">The plaintext Content Encryption Key (CEK).</param>
    /// <param name="iv">The initialization vector.</param>
    /// <param name="inputStream">The <see cref="Stream"/> representing the encrypted data to be decrypted.</param>
    /// <param name="outputStream">A writable <see cref="Stream"/> where the decrypted content will be written.</param>
    /// <returns>Returns a task that represents the asynchronous decryption operation.</returns>
    Task DecryptContent(byte[] key, byte[] iv, Stream inputStream, Stream outputStream);
}