namespace GymPro.Domain.Memberships;

/// <summary>
/// Lifecycle of a subscription. The original app coded this as a raw byte and
/// compared it inline, so every status check was an undocumented magic number.
/// </summary>
public enum MemberStatus : byte
{
    Active = 0,
    Expired = 1,
    Inactive = 2,
    Banned = 3,
}
