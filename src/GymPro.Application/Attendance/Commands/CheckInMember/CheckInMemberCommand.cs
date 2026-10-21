using GymPro.Application.Abstractions;
using GymPro.Domain.Attendance;
using GymPro.Domain.Common;
using GymPro.Domain.Memberships;
using GymPro.Domain.ValueObjects;

namespace GymPro.Application.Attendance.Commands.CheckInMember;

/// <summary>
/// Records a member's arrival. The eligibility rules live in the domain; this
/// handler loads the aggregate and turns a refusal into a
/// <see cref="Result{T}"/> instead of letting an exception escape to the UI.
/// </summary>
public sealed record CheckInMemberCommand(int MembershipId) : ICommand<Result<CheckInReceipt>>;

/// <summary>What the receptionist sees after a successful check-in.</summary>
public sealed record CheckInReceipt(string MemberName, string CheckedInOn, int TotalToday);

public sealed class CheckInMemberHandler(
    IMembershipAggregateLoader memberships,
    ICheckInWriteRepository checkIns,
    IUnitOfWork unitOfWork,
    TimeProvider clock)
    : ICommandHandler<CheckInMemberCommand, Result<CheckInReceipt>>
{
    public async Task<Result<CheckInReceipt>> Handle(
        CheckInMemberCommand request,
        CancellationToken cancellationToken)
    {
        var membership = await memberships.LoadAsync(request.MembershipId, cancellationToken);
        if (membership is null)
        {
            return Result.Failure<CheckInReceipt>(
                DomainError.NotFound("membership", request.MembershipId));
        }

        var today = JalaliDate.Today(clock);

        CheckIn checkIn;
        try
        {
            checkIn = CheckIn.Record(membership, today, clock);
        }
        catch (InvalidOperationException ex)
        {
            // A lapsed member is a normal outcome at the front desk, not a bug.
            return Result.Failure<CheckInReceipt>(
                DomainError.Validation("membership", ex.Message));
        }
        catch (ArgumentException ex)
        {
            return Result.Failure<CheckInReceipt>(
                DomainError.Validation("checkedInOn", ex.Message));
        }

        checkIns.Add(checkIn);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var totalToday = await checkIns.CountForDateAsync(today, cancellationToken);
        return Result.Success(new CheckInReceipt(membership.FullName, today.ToString(), totalToday));
    }
}
