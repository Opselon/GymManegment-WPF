using GymPro.Domain.Common;
using GymPro.Domain.ValueObjects;

namespace GymPro.Domain.Memberships;

/// <summary>
/// One payment against a membership. An entity, not a value object: it has its
/// own identity in the ledger and can never be edited after the fact.
/// </summary>
public sealed class Payment : Entity
{
    internal Payment(int id, Money amount, JalaliDate paidOn)
    {
        Id = id;
        Amount = amount;
        PaidOn = paidOn;
    }

    public Money Amount { get; }
    public JalaliDate PaidOn { get; }
}
