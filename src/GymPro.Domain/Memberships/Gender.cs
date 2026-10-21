namespace GymPro.Domain.Memberships;

/// <summary>Biological sex of a member. The original schema used byte flags with no named meaning.</summary>
public enum Gender : byte
{
    Male = 0,
    Female = 1,
}
