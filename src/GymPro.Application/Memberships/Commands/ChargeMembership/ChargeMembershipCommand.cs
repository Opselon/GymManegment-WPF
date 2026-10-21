using GymPro.Application.Abstractions;
using GymPro.Domain.Common;
using GymPro.Domain.Memberships;
using GymPro.Domain.ValueObjects;

namespace GymPro.Application.Memberships.Commands.ChargeMembership;

/// <summary>
/// Records a payment and, when the balance reaches zero, extends the
/// subscription from its current end date.
/// </summary>
public sealed record ChargeMembershipCommand(
    int MembershipId,
    decimal AmountRial) : ICommand<Result<ChargeResult>>;

public sealed record ChargeResult(decimal PaidRial, decimal OutstandingRial, JalaliDate NewEndDate);
