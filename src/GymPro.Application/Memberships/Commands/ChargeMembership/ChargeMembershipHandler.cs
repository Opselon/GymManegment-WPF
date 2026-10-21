using GymPro.Application.Abstractions;
using GymPro.Domain.Common;
using GymPro.Domain.Memberships;
using GymPro.Domain.ValueObjects;

namespace GymPro.Application.Memberships.Commands.ChargeMembership;

/// <summary>
/// Records a payment against a membership, and reactivates the subscription
/// when it had lapsed.
/// </summary>
public sealed class ChargeMembershipHandler(
    IMembershipAggregateLoader loader,
    IUnitOfWork unitOfWork,
    TimeProvider clock)
    : ICommandHandler<ChargeMembershipCommand, Result<ChargeResult>>
{
    public async Task<Result<ChargeResult>> Handle(
        ChargeMembershipCommand request,
        CancellationToken cancellationToken)
    {
        var membership = await loader.LoadAsync(request.MembershipId, cancellationToken);
        if (membership is null)
        {
            return Result.Failure<ChargeResult>(DomainError.NotFound("membership", request.MembershipId));
        }

        if (request.AmountRial < 0)
        {
            return Result.Failure<ChargeResult>(DomainError.Validation("amount", "Amount cannot be negative."));
        }

        membership.RecordPayment(
            Money.FromRial(request.AmountRial),
            JalaliDate.Today(clock),
            clock);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(new ChargeResult(
            request.AmountRial,
            membership.OutstandingBalance.RialAmount,
            membership.EndOfCoverage));
    }
}
