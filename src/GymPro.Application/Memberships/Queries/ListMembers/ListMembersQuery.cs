using GymPro.Application.Abstractions;
using GymPro.Domain.Memberships;

namespace GymPro.Application.Memberships.Queries.ListMembers;

/// <summary>Read model for the members list screen.</summary>
public sealed record MemberListItem(
    int Id,
    string FullName,
    string NationalCode,
    string? PhoneNumber,
    string Status,
    string StartDate,
    string EndDate,
    decimal OutstandingRial);

/// <summary>Lists members, optionally filtered by status and search text.</summary>
public sealed record ListMembersQuery(string? SearchText, string? StatusFilter) : IQuery<IReadOnlyList<MemberListItem>>;
