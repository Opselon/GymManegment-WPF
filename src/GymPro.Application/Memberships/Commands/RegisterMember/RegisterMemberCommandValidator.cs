using FluentValidation;
using GymPro.Application.Abstractions;
using GymPro.Domain.Common;
using GymPro.Domain.Memberships;
using GymPro.Domain.ValueObjects;

namespace GymPro.Application.Memberships.Commands.RegisterMember;

/// <summary>
/// Validates a registration request before it reaches the aggregate.
/// </summary>
public sealed class RegisterMemberCommandValidator : AbstractValidator<RegisterMemberCommand>
{
    public RegisterMemberCommandValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MinimumLength(2);
        RuleFor(x => x.NationalCode).NotEmpty().Length(10);
        RuleFor(x => x.DurationMonths).InclusiveBetween(1, 36);
        RuleFor(x => x.MonthlyFeeRial).GreaterThanOrEqualTo(0);
    }
}
