using System.Security.Cryptography;
using System.Text;

namespace SolarCharge.API.Infrastructure.Database.Crypto;

public static class AesEncryption
{
    public static string Encrypt(string plainText, string key)
    {
        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        var keyBytes = DeriveKeyFromString(key, 32); // AES-256 requires 32-byte key

        using var aes = Aes.Create();
        aes.Key = keyBytes;
        aes.GenerateIV(); // Generate random IV for each encryption

        using var encryptor = aes.CreateEncryptor();
        using var ms = new MemoryStream();
        // Prepend IV to encrypted data
        ms.Write(aes.IV, 0, aes.IV.Length);
                
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        {
            cs.Write(plainBytes, 0, plainBytes.Length);
        }
                
        return Convert.ToBase64String(ms.ToArray());
    }
    
    public static string Decrypt(string encryptedText, string key)
    {
        var encryptedBytes = Convert.FromBase64String(encryptedText);
        var keyBytes = DeriveKeyFromString(key, 32);

        using var aes = Aes.Create();
        aes.Key = keyBytes;
            
        // Extract IV from the beginning of encrypted data
        var iv = new byte[16]; // AES block size is 16 bytes
        Array.Copy(encryptedBytes, 0, iv, 0, 16);
        aes.IV = iv;
            
        // Get the actual encrypted data (skip IV)
        var cipherText = new byte[encryptedBytes.Length - 16];
        Array.Copy(encryptedBytes, 16, cipherText, 0, cipherText.Length);

        using var decryptor = aes.CreateDecryptor();
        using var ms = new MemoryStream(cipherText);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var reader = new StreamReader(cs);
        return reader.ReadToEnd();
    }
    
    private static byte[] DeriveKeyFromString(string key, int keySize)
    {
        // Use PBKDF2 to derive a proper key from string
        var salt = "SuperSecureSaltValue46290"u8.ToArray();
        using var rfc2898 = new Rfc2898DeriveBytes(key, salt, 10000, HashAlgorithmName.SHA256);
        return rfc2898.GetBytes(keySize);
    }
}