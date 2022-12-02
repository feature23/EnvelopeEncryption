namespace F23.EnvelopeEncryption;

/// <summary>
/// A type that wraps an initialization vector byte array.
/// </summary>
/// <param name="Value"></param>
public record InitializationVector(byte[] Value)
{
    /// <summary>
    /// Implicitly converts the <see cref="InitializationVector"/> to a byte array by unwrapping its value.
    /// </summary>
    /// <param name="iv"></param>
    /// <returns></returns>
    public static implicit operator byte[](InitializationVector iv) => iv.Value;
}