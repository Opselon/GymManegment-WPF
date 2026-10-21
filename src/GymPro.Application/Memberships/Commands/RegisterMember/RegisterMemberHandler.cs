using GymPro.Application.Abstractions;
using GymPro.Domain.Common;
using GymPro.Domain.Memberships;
using GymPro.Domain.ValueObjects;

namespace GymPro.Application.Memberships.Commands.RegisterMember;

/// <summary>
/// Registers a new member. Kept deliberately thin: the aggregate owns the
/// business rules, so this handler is the transaction boundary and nothing more.
/// </summary>
public sealed class RegisterMemberHandler(
    IMembershipWriteRepository repository,
    IUnitOfWork unitOfWork,
    TimeProvider clock)
    : ICommandHandler<RegisterMemberCommand, Result<int>>
{
    public async Task<Result<int>> Handle(RegisterMemberCommand request, CancellationToken cancellationToken)
    {
        // Pre-checks beyond field-shape validation (uniqueness, date validity).
        if (await repository.ExistsNationalCodeAsync(request.NationalCode, cancellationToken))
        {
            return Result.Failure<int>(DomainError.Conflict("nationalCode", "A member with this national code already exists."));
        }

        if (!JalaliDate.TryParse($"{request.StartYear:D4}-{request.StartMonth:D2}-{request.StartDay:D2}", out var startDate))
        {
            return Result.Failure<int>(DomainError.Validation("startDate", "Start date is not a valid Jalali date."));
        }

        if (startDate > JalaliDate.Today(clock))
        {
            return Result.Failure<int>(DomainError.Validation("startDate", "Start date cannot be in the future."));
        }

        var member = Membership.RegisterNew(
            fullName: request.FullName,
            gender: (Gender)request.Gender,
            startDate: startDate,
            durationMonths: request.DurationMonths,
            monthlyFee: Money.FromRial(request.MonthlyFeeRial),
            nationalCode: request.NationalCode,
            phoneNumber: request.PhoneNumber,
            clock: clock);

        repository.Add(member);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(member.Id);
    }
}
