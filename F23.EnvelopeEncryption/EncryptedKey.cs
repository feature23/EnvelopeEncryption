namespace F23.EnvelopeEncryption;

/// <summary>
/// An encrypted Content Encryption Key (CEK).
/// </summary>
public class EncryptedKey
{
    /// <summary>
    /// Creates a new EncryptedKey instance.
    /// </summary>
    /// <param name="keyId">The identifier of the Key Encryption Key (KEK) used to encrypt this CEK.</param>
    /// <param name="key">The encrypted (ciphertext) CEK.</param>
    /// <param name="algorithm">The algorithm used to encrypt this CEK.</param>
    public EncryptedKey(string keyId, byte[] key, string algorithm)
    {
        KeyId = keyId;
        Key = key;
        Algorithm = algorithm;
    }

    /// <summary>
    /// The identifier of the Key Encryption Key (KEK) used to encrypt this CEK.
    /// </summary>
    public string KeyId { get; }
    
    /// <summary>
    /// The encrypted (ciphertext) CEK.
    /// </summary>
    public byte[] Key { get; }
    
    /// <summary>
    /// The algorithm used to encrypt this CEK.
    /// </summary>
    public string Algorithm { get; }

    /// <summary>
    /// Determines if this instance equals the provided <paramref name="other"/> instance.
    /// </summary>
    /// <param name="other">Another EncryptedKey to compare.</param>
    /// <returns>Returns true if equal.</returns>
    protected bool Equals(EncryptedKey other)
    {
        return KeyId == other.KeyId && Key.Equals(other.Key) && Algorithm == other.Algorithm;
    }

    /// <summary>
    /// Determines if this instance equals the provided <paramref name="obj"/> instance.
    /// </summary>
    /// <param name="obj">Another object to compare.</param>
    /// <returns>Returns true if equal.</returns>
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((EncryptedKey)obj);
    }

    /// <summary>
    /// Gets the hash code of this instance.
    /// </summary>
    /// <returns>Returns a hash code.</returns>
    public override int GetHashCode() => HashCode.Combine(KeyId, Key, Algorithm);

    /// <summary>
    /// Gets the string representation of this instance.
    /// </summary>
    /// <returns>Returns a non-null string.</returns>
    public override string ToString() => "{Encrypted Key}";
}