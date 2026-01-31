using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace DairyFamilyManager.Services;

public class PasswordService
{
    public string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);

        byte[] hash = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 32
        );

        return Convert.ToBase64String(salt) + "." + Convert.ToBase64String(hash);
    }

    public bool VerifyPassword(string password, string stored)
    {
        if (string.IsNullOrWhiteSpace(stored)) return false;

        string[] parts = stored.Split('.', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2) return false;

        byte[] salt = Convert.FromBase64String(parts[0]);
        byte[] storedHash = Convert.FromBase64String(parts[1]);

        byte[] computed = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 32
        );

        return CryptographicOperations.FixedTimeEquals(storedHash, computed);
    }
}
