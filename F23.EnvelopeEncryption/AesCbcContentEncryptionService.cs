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
    /// <returns>Returns a new <see cref="EncryptedValue"/> instance.</returns>
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
    /// Encrypts the provided binary content <paramref name="value" /> using the given plaintext <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The plaintext Content Encryption Key (CEK).</param>
    /// <param name="value">The plaintext byte array value to encrypt.</param>
    /// <returns>Returns a new <see cref="EncryptedValue"/> instance.</returns>
    public EncryptedValue EncryptContent(byte[] key, byte[] value)
    {
        var aes = Aes.Create();
        aes.Key = key;
        
        aes.GenerateIV();
        var iv = aes.IV;
        
        var ciphertext = aes.EncryptCbc(value, iv, AesCbcPaddingMode);
        
        return new EncryptedValue(iv, ciphertext);
    }

    /// <summary>
    /// Encrypts the provided plaintext <paramref name="inputStream"/> to the provided <paramref name="outputStream"/>.
    /// </summary>
    /// <param name="key">The plaintext Content Encryption Key (CEK).</param>
    /// <param name="inputStream">The <see cref="Stream"/> representing the plaintext data to be encrypted.</param>
    /// <param name="outputStream">The <see cref="Stream"/> where the encrypted content will be written.</param>
    /// <returns>Returns a task that represents the asynchronous decryption operation.</returns>
    public async Task<InitializationVector> EncryptContent(byte[] key, Stream inputStream, Stream outputStream)
    {
        var aes = Aes.Create();
        aes.Key = key;
        aes.Mode = CipherMode.CBC;
        aes.Padding = AesCbcPaddingMode;
        
        aes.GenerateIV();
        var iv = aes.IV;
        
        using var encryptor = aes.CreateEncryptor();
        await using var cryptoStream = new CryptoStream(outputStream, encryptor, CryptoStreamMode.Write, leaveOpen: true);

        await inputStream.CopyToAsync(cryptoStream);

        return new InitializationVector(iv);
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

    /// <summary>
    /// Decrypts the provided encrypted <paramref name="value"/> using the given plaintext <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The plaintext Content Encryption Key (CEK).</param>
    /// <param name="value">The <see cref="EncryptedValue"/> to decrypt.</param>
    /// <returns>Returns the decrypted content byte array.</returns>
    public byte[] DecryptContent(byte[] key, EncryptedValue value)
    {
        var aes = Aes.Create();
        aes.Key = key;
        aes.IV = value.Iv;

        return aes.DecryptCbc(value.Ciphertext, value.Iv, AesCbcPaddingMode);
    }
    
    /// <summary>
    /// Decrypts the provided encrypted <paramref name="inputStream"/> to the provided <paramref name="outputStream"/>
    /// using the given plaintext <paramref name="key"/> and <paramref name="iv"/>.
    /// </summary>
    /// <param name="key">The plaintext Content Encryption Key (CEK).</param>
    /// <param name="iv">The initialization vector.</param>
    /// <param name="inputStream">The <see cref="Stream"/> representing the encrypted data to be decrypted.</param>
    /// <param name="outputStream">A writable <see cref="Stream"/> where the decrypted content will be written.</param>
    /// <returns>Returns a task that represents the asynchronous decryption operation.</returns>
    public async Task DecryptContent(byte[] key, byte[] iv, Stream inputStream, Stream outputStream)
    {
        var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = AesCbcPaddingMode;

        using var decryptor = aes.CreateDecryptor();
        await using var cryptoStream = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read);
        
        await cryptoStream.CopyToAsync(outputStream);
    }
}