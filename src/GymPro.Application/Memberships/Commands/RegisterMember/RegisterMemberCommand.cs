using GymPro.Application.Abstractions;
using GymPro.Domain.Common;
using GymPro.Domain.Memberships;

namespace GymPro.Application.Memberships.Commands.RegisterMember;

/// <summary>
/// Registers a new member and opens their subscription.
/// </summary>
public sealed record RegisterMemberCommand(
    string FullName,
    byte Gender,
    string NationalCode,
    string? PhoneNumber,
    int StartYear,
    int StartMonth,
    int StartDay,
    int DurationMonths,
    decimal MonthlyFeeRial) : ICommand<Result<int>>;
