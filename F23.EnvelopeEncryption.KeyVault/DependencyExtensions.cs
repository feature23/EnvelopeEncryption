using F23.EnvelopeEncryption.KeyVault;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace F23.EnvelopeEncryption;

/// <summary>
/// Extension methods for configuring the Key Vault extensions to Envelope Encryption.
/// </summary>
public static class DependencyExtensions
{
    /// <summary>
    /// Adds the Key Vault extensions for Envelope Encryption.
    /// </summary>
    /// <param name="builder">The Envelope Encryption Builder to configure.</param>
    /// <returns>Returns an Envelope Encryption Builder instance.</returns>
    public static EnvelopeEncryptionBuilder AddKeyVault(this EnvelopeEncryptionBuilder builder)
    {
        builder.Services.AddTransient<IContentKeyEncryptionService, KeyVaultContentKeyEncryptionService>();

        builder.Services.AddOptions<KeyVaultEnvelopeEncryptionOptions>()
            .Configure<IConfiguration>((options, config) => config.GetSection(KeyVaultEnvelopeEncryptionOptions.Options).Bind(options));

        return builder;
    }
}