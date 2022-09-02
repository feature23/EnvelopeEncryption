namespace F23.EnvelopeEncryption.KeyVault;

/// <summary>
/// Options for configuring the Key Vault extensions to Envelope Encryption.
/// </summary>
public class KeyVaultEnvelopeEncryptionOptions
{
    /// <summary>
    /// The name of the options path.
    /// </summary>
    public const string Options = "EnvelopeEncryption:KeyVault";

    /// <summary>
    /// The name of the Key Encryption Key (KEK) to use to encrypt Content Encryption Keys (CEKs).
    /// </summary>
    public string KeyEncryptionKeyName { get; set; } = string.Empty;
    
    /// <summary>
    /// Optional. The specific version of the Key Encryption Key (KEK) to use to encrypt Content Encryption Keys (CEKs).
    /// If not provided (or is null), this will use the latest version of the key.
    /// </summary>
    public string? KeyEncryptionKeyVersion { get; set; }
}