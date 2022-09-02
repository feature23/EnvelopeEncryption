using Microsoft.Extensions.DependencyInjection;

namespace F23.EnvelopeEncryption;

/// <summary>
/// Extension method for configuring envelope encryption with dependency injection.
/// </summary>
public static class DependencyExtensions
{
    /// <summary>
    /// Adds default dependencies to enable Envelope Encryption.
    ///
    /// Note: This does not register an implementation of <see cref="IContentKeyEncryptionService"/>.
    /// It is recommended to use an extension to this library like F23.EnvelopeEncryption.KeyVault,
    /// or to provide your own implementation.
    /// </summary>
    /// <param name="services">The current services collection.</param>
    /// <returns>Returns an <see cref="EnvelopeEncryptionBuilder"/> for further configuration.</returns>
    public static EnvelopeEncryptionBuilder AddEnvelopeEncryption(this IServiceCollection services)
    {
        // TODO: allow configuring for other algorithms
        services.AddTransient<IContentEncryptionService, AesCbcContentEncryptionService>();
        services.AddTransient<IEnvelopeEncryptionService, EnvelopeEncryptionService>();
        
        return new EnvelopeEncryptionBuilder(services);
    }
}