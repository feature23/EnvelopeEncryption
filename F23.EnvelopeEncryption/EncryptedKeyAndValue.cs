namespace F23.EnvelopeEncryption;

/// <summary>
/// A convenience type for storing both an <see cref="EncryptedKey"/> and <see cref="EncryptedValue"/> together.
/// </summary>
public class EncryptedKeyAndValue
{
    /// <summary>
    /// Creates a new EncryptedKeyAndValue instance.
    /// </summary>
    /// <param name="key">An EncryptedKey instance.</param>
    /// <param name="value">An EncryptedValue instance.</param>
    public EncryptedKeyAndValue(EncryptedKey key, EncryptedValue value)
    {
        Key = key;
        Value = value;
    }

    /// <summary>
    /// The encrypted Content Encryption Key.
    /// </summary>
    public EncryptedKey Key { get; }
    
    /// <summary>
    /// The encrypted value.
    /// </summary>
    public EncryptedValue Value { get; }

    /// <summary>
    /// Determines if this instance equals the provided <paramref name="other"/> instance.
    /// </summary>
    /// <param name="other">Another EncryptedKeyAndValue to compare.</param>
    /// <returns>Returns true if equal.</returns>
    protected bool Equals(EncryptedKeyAndValue other)
    {
        return Key.Equals(other.Key) && Value.Equals(other.Value);
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
        return Equals((EncryptedKeyAndValue)obj);
    }

    /// <summary>
    /// Gets the hash code of this instance.
    /// </summary>
    /// <returns>Returns a hash code.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Key, Value);
    }

    /// <summary>
    /// Gets the string representation of this instance.
    /// </summary>
    /// <returns>Returns a non-null string.</returns>
    public override string ToString() => "{Encrypted Key and Value}";
}