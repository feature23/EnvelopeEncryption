namespace F23.EnvelopeEncryption;

/// <summary>
/// An encrypted content value.
/// </summary>
public class EncryptedValue
{
    /// <summary>
    /// Creates a new EncryptedValue instance.
    /// </summary>
    /// <param name="iv">The Initialization Vector (IV) used to encrypt this value.</param>
    /// <param name="ciphertext">The encrypted ciphertext of this value.</param>
    public EncryptedValue(byte[] iv, byte[] ciphertext)
    {
        Iv = iv;
        Ciphertext = ciphertext;
    }

    /// <summary>
    /// The Initialization Vector (IV) used to encrypt this value.
    /// </summary>
    public byte[] Iv { get; }

    /// <summary>
    /// The encrypted ciphertext of this value.
    /// </summary>
    public byte[] Ciphertext { get; }

    /// <summary>
    /// Determines if this instance equals the provided <paramref name="other"/> instance.
    /// </summary>
    /// <param name="other">Another EncryptedValue to compare.</param>
    /// <returns>Returns true if equal.</returns>
    protected bool Equals(EncryptedValue other)
    {
        return Iv.Equals(other.Iv) && Ciphertext.Equals(other.Ciphertext);
    }

    /// <summary>
    /// Determines if this instance equals the provided <paramref name="obj"/> instance.
    /// </summary>
    /// <param name="obj">Another obj to compare.</param>
    /// <returns>Returns true if equal.</returns>
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((EncryptedValue)obj);
    }

    /// <summary>
    /// Gets the hash code of this instance.
    /// </summary>
    /// <returns>Returns a hash code.</returns>
    public override int GetHashCode() => HashCode.Combine(Iv, Ciphertext);

    /// <summary>
    /// Gets the string representation of this instance.
    /// </summary>
    /// <returns>Returns a non-null string.</returns>
    public override string ToString() => "{Encrypted Value}";
}
