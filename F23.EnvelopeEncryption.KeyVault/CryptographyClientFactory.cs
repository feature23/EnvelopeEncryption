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
    /// /// <param name="credentialOptions">Optional <see cref="DefaultAzureCredentialOptions"/> to customize the authentication process.</param>
    /// <returns>Returns a new <see cref="CryptographyClient"/> instance.</returns>
    public CryptographyClient CreateFromKeyUri(string uri, DefaultAzureCredentialOptions? credentialOptions = null)
        => CreateFromKeyUri(new Uri(uri), credentialOptions);

    /// <summary>
    /// Creates a new <see cref="CryptographyClient"/> from the given key <paramref name="uri"/>.
    /// </summary>
    /// <param name="uri">The URI of the key.</param>
    /// <param name="credentialOptions">Optional <see cref="DefaultAzureCredentialOptions"/> to customize the authentication process.</param>
    /// <returns>Returns a new <see cref="CryptographyClient"/> instance.</returns>
    public CryptographyClient CreateFromKeyUri(Uri uri, DefaultAzureCredentialOptions? credentialOptions = null)
        => new(uri, new DefaultAzureCredential(credentialOptions));
}