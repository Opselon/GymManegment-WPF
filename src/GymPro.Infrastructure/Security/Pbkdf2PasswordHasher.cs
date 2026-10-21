using System.Security.Cryptography;
using GymPro.Application.Abstractions;
using Microsoft.Extensions.Options;

namespace GymPro.Infrastructure.Security;

/// <summary>
/// Password hashing options.
/// </summary>
public sealed class PasswordHasherOptions
{
    /// <summary>Salt size in bytes. 16 is the modern default.</summary>
    public int SaltSize { get; set; } = 16;

    /// <summary>Hash size in bytes. 32 gives 256-bit output.</summary>
    public int HashSize { get; set; } = 32;

    /// <summary>PBKDF2 iterations. Raised over time as hardware improves.</summary>
    public int Iterations { get; set; } = 210_000;
}

/// <summary>
/// Passwords are hashed with PBKDF2 / HMAC-SHA512 and stored as
/// "pbkdf2-sha512.{iterations}.{salt_base64}.{hash_base64}" so the iteration
/// count can be raised per-user on the next successful login.
///
/// The original application stored passwords encrypted with 3DES-ECB keyed by
/// an MD5 of a config value: reversible, identical plaintexts produced identical
/// ciphertexts, and the key sat next to the data in the same config file.
/// </summary>
public sealed class Pbkdf2PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasherOptions _options;

    public Pbkdf2PasswordHasher(IOptions<PasswordHasherOptions> options)
        => _options = options.Value;

    public string Hash(string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("Password is required.", nameof(password));

        var salt = RandomNumberGenerator.GetBytes(_options.SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, _options.Iterations, HashAlgorithmName.SHA512, _options.HashSize);
        return $"pbkdf2-sha512.{_options.Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    public bool Verify(string password, string storedHash)
    {
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash))
            return false;

        var parts = storedHash.Split('.');
        // parts[0]=algorithm parts[1]=iterations parts[2]=salt parts[3]=hash
        if (parts.Length != 4 || parts[0] != "pbkdf2-sha512")
            return false;

        if (!int.TryParse(parts[1], out var iterations) || iterations < 1)
            return false;

        byte[] salt;
        byte[] expected;
        try
        {
            salt = Convert.FromBase64String(parts[2]);
            expected = Convert.FromBase64String(parts[3]);
        }
        catch (FormatException)
        {
            return false;
        }

        var actual = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations,
            HashAlgorithmName.SHA512,
            expected.Length);

        return CryptographicOperations.FixedTimeEquals(actual, expected);
    }
}
