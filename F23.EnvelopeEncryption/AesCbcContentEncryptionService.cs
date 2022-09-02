using System.Security.Cryptography;
using System.Text;

namespace F23.EnvelopeEncryption;

/// <summary>
/// An implementation of <see cref="IContentEncryptionService"/> that uses AES-CBC symmetric encryption.
/// </summary>
public class AesCbcContentEncryptionService : IContentEncryptionService
{
    private const PaddingMode AesCbcPaddingMode = PaddingMode.PKCS7;
    private const int KeySizeBytes = 32;

    /// <summary>
    /// Creates a new plaintext (unencrypted) Content Encryption Key (CEK).
    /// A new CEK should be generated for each content value.
    /// Warning: This plaintext CEK is unencrypted, and must be encrypted via the
    /// <see cref="IEnvelopeEncryptionService"/> before persisting to storage.
    /// </summary>
    /// <returns>Returns a new non-null byte array containing the new CEK.</returns>
    public byte[] CreateContentEncryptionKey()
    {
        return RandomNumberGenerator.GetBytes(KeySizeBytes);
    }

    /// <summary>
    /// Encrypts the provided content <paramref name="value"/> string using the given plaintext <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The plaintext Content Encryption Key (CEK).</param>
    /// <param name="value">The plaintext string value to encrypt.</param>
    /// <returns>Returns a new EncryptedValue instance.</returns>
    public EncryptedValue EncryptContentString(byte[] key, string value)
    {
        var aes = Aes.Create();
        aes.Key = key;
        
        aes.GenerateIV();
        var iv = aes.IV;
        
        var plaintext = Encoding.UTF8.GetBytes(value);

        var ciphertext = aes.EncryptCbc(plaintext, iv, AesCbcPaddingMode);
        
        return new EncryptedValue(iv, ciphertext);
    }

    /// <summary>
    /// Decrypts the provided encrypted <paramref name="value"/> using the given plaintext <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The plaintext Content Encryption Key (CEK).</param>
    /// <param name="value">The encrypted value to decrypt.</param>
    /// <returns>Returns the decrypted content string.</returns>
    public string DecryptContentString(byte[] key, EncryptedValue value)
    {
        var aes = Aes.Create();
        aes.Key = key;
        aes.IV = value.Iv;

        var plaintext = aes.DecryptCbc(value.Ciphertext, value.Iv, AesCbcPaddingMode);

        return Encoding.UTF8.GetString(plaintext);
    }
}