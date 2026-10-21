using GymPro.Domain.Memberships;
using GymPro.Domain.ValueObjects;

namespace GymPro.Domain.Attendance;

/// <summary>
/// One check-in by a member. Written once and never edited, which makes the
/// table an immutable audit log of gym usage.
/// </summary>
public sealed class CheckIn
{
    internal CheckIn(int id, int membershipId, JalaliDate checkedInOn)
    {
        Id = id;
        MembershipId = membershipId;
        CheckedInOn = checkedInOn;
    }

    public int Id { get; }
    public int MembershipId { get; }
    public JalaliDate CheckedInOn { get; }

    /// <summary>
    /// Records a check-in for a member.
    /// </summary>
    /// <remarks>
    /// The aggregate is passed in rather than its id so the eligibility rule
    /// cannot be bypassed by a caller that only has an integer: an expired,
    /// inactive or banned member must not be able to enter the gym.
    /// </remarks>
    /// <exception cref="ArgumentException">The check-in is dated in the future.</exception>
    /// <exception cref="InvalidOperationException">The membership cannot attend on that date.</exception>
    public static CheckIn Record(Membership membership, JalaliDate on, TimeProvider clock)
    {
        ArgumentNullException.ThrowIfNull(membership);

        if (on > JalaliDate.Today(clock))
        {
            throw new ArgumentException("A check-in cannot be dated in the future.", nameof(on));
        }

        if (membership.Status is MemberStatus.Inactive or MemberStatus.Banned or MemberStatus.Expired)
        {
            throw new InvalidOperationException(
                $"Membership {membership.Id} is {membership.Status} and cannot check in.");
        }

        if (membership.IsExpired(on))
        {
            throw new InvalidOperationException(
                $"Membership {membership.Id} expired on {membership.EndOfCoverage} and cannot check in.");
        }

        return new CheckIn(0, membership.Id, on);
    }
}

/// <summary>
/// Body measurements recorded over time. Tracked per member so progress
/// can be plotted; the original app kept these as loose string columns.
/// </summary>
public sealed class BodyMeasurement
{
    internal BodyMeasurement(
        int id,
        int membershipId,
        JalaliDate measuredOn,
        int? chestCm,
        int? armCm,
        int? waistCm,
        int? heightCm,
        int? weightKg)
    {
        Id = id;
        MembershipId = membershipId;
        MeasuredOn = measuredOn;
        ChestCm = chestCm;
        ArmCm = armCm;
        WaistCm = waistCm;
        HeightCm = heightCm;
        WeightKg = weightKg;
    }

    /// <summary>Records a measurement for a member on a given day.</summary>
    public static BodyMeasurement Record(
        int membershipId,
        JalaliDate measuredOn,
        int? chestCm = null,
        int? armCm = null,
        int? waistCm = null,
        int? heightCm = null,
        int? weightKg = null)
    {
        if (chestCm is < 0 || armCm is < 0 || waistCm is < 0 || heightCm is < 0 || weightKg is < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(chestCm), "Measurements cannot be negative.");
        }

        return new BodyMeasurement(0, membershipId, measuredOn, chestCm, armCm, waistCm, heightCm, weightKg);
    }

    public int Id { get; }
    public int MembershipId { get; }
    public JalaliDate MeasuredOn { get; }
    public int? ChestCm { get; }
    public int? ArmCm { get; }
    public int? WaistCm { get; }
    public int? HeightCm { get; }
    public int? WeightKg { get; }
}
