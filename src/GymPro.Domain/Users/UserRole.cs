namespace GymPro.Domain.Users;

/// <summary>
/// Application roles. The original schema had a single byte flag with no
/// documented meaning, so every permission check was a magic number.
/// </summary>
public enum UserRole : byte
{
    Admin = 0,
    Coach = 1,
    Receptionist = 2,
}
