using GymPro.Domain.Common;
using GymPro.Domain.ValueObjects;

namespace GymPro.Domain.Memberships;

/// <summary>
/// The membership aggregate root: one member plus the subscription they hold.
/// </summary>
public sealed class Membership : HasDomainEvents
{
    private readonly List<Payment> _payments = [];

    internal Membership(
        int id,
        string fullName,
        Gender gender,
        JalaliDate startDate,
        int durationMonths,
        Money monthlyFee,
        MemberStatus status)
    {
        Id = id;
        FullName = fullName;
        Gender = gender;
        StartDate = startDate;
        DurationMonths = durationMonths;
        MonthlyFee = monthlyFee;
        Status = status;
    }

    public static Membership RegisterNew(
        string fullName,
        Gender gender,
        JalaliDate startDate,
        int durationMonths,
        Money monthlyFee,
        string? nationalCode,
        string? phoneNumber,
        TimeProvider clock)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("Full name is required.", nameof(fullName));
        }

        if (string.IsNullOrWhiteSpace(nationalCode) || nationalCode!.Trim().Length != 10)
        {
            throw new ArgumentException("National code must be 10 digits.", nameof(nationalCode));
        }

        if (durationMonths is < 1 or > 36)
        {
            throw new ArgumentOutOfRangeException(nameof(durationMonths), "Duration must be between 1 and 36 months.");
        }

        var member = new Membership(
            id: 0,
            fullName: fullName.Trim(),
            gender: gender,
            startDate: startDate,
            durationMonths: durationMonths,
            monthlyFee: monthlyFee,
            status: MemberStatus.Active);
        member.NationalCode = nationalCode.Trim();
        member.PhoneNumber = phoneNumber;
        member.RegisteredOn = JalaliDate.Today(clock);

        member.Raise(new MemberRegistered(member.FullName));
        return member;
    }

    public string FullName { get; internal set; }
    public Gender Gender { get; internal set; }
    public string? NationalCode { get; internal set; }
    public string? PhoneNumber { get; internal set; }
    public JalaliDate StartDate { get; internal set; }
    public int DurationMonths { get; internal set; }
    public Money MonthlyFee { get; internal set; }
    public MemberStatus Status { get; internal set; }
    public JalaliDate RegisteredOn { get; internal set; }
    public byte[]? Photo { get; internal set; }

    /// <summary>First day the member may attend.</summary>
    public JalaliDate StartOfCoverage => StartDate;

    /// <summary>Last day the subscription is paid for.</summary>
    public JalaliDate EndOfCoverage => StartDate.AddMonths(DurationMonths);

    public bool IsExpired(JalaliDate asOf) => asOf > EndOfCoverage;

    /// <summary>Amount the member still owes for the current subscription period.</summary>
    public Money OutstandingBalance
    {
        get
        {
            var total = MonthlyFee * DurationMonths;
            var paid = _payments.Aggregate(Money.Zero, (acc, p) => acc + p.Amount);
            return total - paid;
        }
    }

    public IReadOnlyList<Payment> Payments => _payments.AsReadOnly();

    public void RecordPayment(Money amount, JalaliDate paymentDate, TimeProvider clock)
    {
        if (amount.IsNegative)
        {
            throw new ArgumentException("Payment amount cannot be negative.", nameof(amount));
        }

        if (paymentDate > JalaliDate.Today(clock))
        {
            throw new ArgumentException("Payment date cannot be in the future.", nameof(paymentDate));
        }

        _payments.Add(new Payment(_payments.Count + 1, amount, paymentDate));
        Raise(new PaymentRecorded(amount.RialAmount));
    }

    /// <summary>
    /// Extends the subscription. The extra months are charged at the current
    /// monthly fee, keeping renewal arithmetic in one place.
    /// </summary>
    public void Renew(int extraMonths, Money monthlyFee, TimeProvider clock)
    {
        if (extraMonths is < 1 or > 36)
        {
            throw new ArgumentOutOfRangeException(nameof(extraMonths), "Duration must be between 1 and 36 months.");
        }

        DurationMonths += extraMonths;
        if (Status == MemberStatus.Expired)
        {
            Status = MemberStatus.Active;
        }

        Raise(new MembershipRenewed(extraMonths));
    }

    /// <summary>
    /// Moves the membership to <see cref="MemberStatus.Expired"/> when its coverage
    /// has lapsed. Idempotent, so a nightly sweep can call it on every member.
    /// </summary>
    /// <returns><see langword="true"/> when the status changed.</returns>
    public bool ExpireIfLapsed(JalaliDate asOf)
    {
        if (Status != MemberStatus.Active || !IsExpired(asOf))
        {
            return false;
        }

        Status = MemberStatus.Expired;
        Raise(new MembershipExpired(EndOfCoverage));
        return true;
    }

    /// <summary>
    /// Assigns identity from the persistence store. Store and test doubles use
    /// this because the database, not the aggregate, allocates row ids.
    /// </summary>
    internal void SetIdForTest(int id) => Id = id;

    public void Deactivate(string reason)
    {
        if (Status == MemberStatus.Inactive)
        {
            return;
        }

        Status = MemberStatus.Inactive;
        Raise(new MembershipDeactivated(reason));
    }
}
