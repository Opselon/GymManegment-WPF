namespace GymPro.Application.Abstractions;

/// <summary>
/// Hashes and verifies passwords. Implemented in Infrastructure with PBKDF2
/// (replacing the original 3DES/MD5 scheme, which stored reversible encryption
/// rather than a hash). The port keeps the algorithm out of the domain and
/// application layers.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>Hashes a plain-text password.</summary>
    string Hash(string password);

    /// <summary>Checks a plain-text password against a stored hash.</summary>
    bool Verify(string password, string storedHash);
}
