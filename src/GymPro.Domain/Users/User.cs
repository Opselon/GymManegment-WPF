using GymPro.Domain.Common;

namespace GymPro.Domain.Users;

/// <summary>
/// A staff or coach account that can sign in to the application.
/// </summary>
public sealed class User : HasDomainEvents
{
    internal User(int id, string username, string displayName, string passwordHash, UserRole role)
    {
        Id = id;
        Username = username;
        DisplayName = displayName;
        PasswordHash = passwordHash;
        Role = role;
    }

    /// <summary>
    /// Registers a new user. Password hashing is NOT performed here: hashing is
    /// infrastructure (PBKDF2 today) and the domain only carries the resulting
    /// string, so the crypto can be swapped without touching this aggregate.
    /// </summary>
    public static User Register(string username, string displayName, string passwordHash, UserRole role)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required.", nameof(username));
        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException("Display name is required.", nameof(displayName));
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash is required.", nameof(passwordHash));

        var user = new User(0, username.Trim(), displayName.Trim(), passwordHash, role);
        user.Raise(new UserRegistered(user.Username));
        return user;
    }

    public string Username { get; internal set; }
    public string DisplayName { get; internal set; }
    public string PasswordHash { get; private set; }
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; } = true;

    public void ChangePassword(string newHash)
    {
        if (string.IsNullOrWhiteSpace(newHash))
            throw new ArgumentException("Password hash is required.", nameof(newHash));
        PasswordHash = newHash;
    }

    public void Suspend()
    {
        if (!IsActive)
            return;
        IsActive = false;
        Raise(new UserSuspended(Username));
    }

    public void Reinstate()
    {
        if (IsActive)
            return;
        IsActive = true;
        Raise(new UserReinstated(Username));
    }
}
