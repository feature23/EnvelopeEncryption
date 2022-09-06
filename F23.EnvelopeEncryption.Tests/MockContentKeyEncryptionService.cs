using System.Security.Cryptography;

namespace F23.EnvelopeEncryption.Tests;

/// <summary>
/// A mock content key encryption service. Warning: This is not suitable for production use!
/// </summary>
internal class MockContentKeyEncryptionService : IContentKeyEncryptionService
{
    private readonly Aes _aes = Aes.Create();
    
    public Task<EncryptedKey> EncryptContentEncryptionKeyAsync(byte[] key, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new EncryptedKey("test", _aes.EncryptCbc(key, _aes.IV), "mock"));
    }

    public Task<byte[]> DecryptContentEncryptionKeyAsync(EncryptedKey key, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_aes.DecryptCbc(key.Key, _aes.IV));
    }
}