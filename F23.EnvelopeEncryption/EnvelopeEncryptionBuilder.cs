using Microsoft.Extensions.DependencyInjection;

namespace F23.EnvelopeEncryption;

/// <summary>
/// A builder for configuring Envelope Encryption.
/// </summary>
public class EnvelopeEncryptionBuilder
{
    /// <summary>
    /// Creates a new EnvelopeEncryptionBuilder instance.
    /// </summary>
    /// <param name="services">The current services collection.</param>
    public EnvelopeEncryptionBuilder(IServiceCollection services)
    {
        Services = services;
    }
    
    /// <summary>
    /// The current services collection.
    /// </summary>
    public IServiceCollection Services { get; }
}