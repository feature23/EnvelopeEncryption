using Azure.Identity;
using Azure.Security.KeyVault.Keys.Cryptography;

namespace F23.EnvelopeEncryption.KeyVault;

/// <summary>
/// A factory for creating <see cref="CryptographyClient"/> instances.
/// </summary>
public class CryptographyClientFactory
{
    /// <summary>
    /// Creates a new <see cref="CryptographyClient"/> from the given key <paramref name="uri"/>.
    /// </summary>
    /// <param name="uri">The URI of the key.</param>
    /// <returns>Returns a new <see cref="CryptographyClient"/> instance.</returns>
    public CryptographyClient CreateFromKeyUri(string uri)
        => CreateFromKeyUri(new Uri(uri));
    
    /// <summary>
    /// Creates a new <see cref="CryptographyClient"/> from the given key <paramref name="uri"/>.
    /// </summary>
    /// <param name="uri">The URI of the key.</param>
    /// <returns>Returns a new <see cref="CryptographyClient"/> instance.</returns>
    public CryptographyClient CreateFromKeyUri(Uri uri)
        => new(uri, new DefaultAzureCredential());
}