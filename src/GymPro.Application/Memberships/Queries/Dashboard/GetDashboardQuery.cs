using GymPro.Application.Abstractions;
using GymPro.Domain.Memberships;
using GymPro.Domain.ValueObjects;

namespace GymPro.Application.Memberships.Queries.Dashboard;

/// <summary>The numbers shown on the main dashboard.</summary>
public sealed record DashboardSummary(
    int TotalMembers,
    int ActiveMembers,
    int ExpiringThisMonth,
    decimal OutstandingTotalRial,
    decimal CollectedThisMonthRial);

public sealed record GetDashboardQuery : IQuery<DashboardSummary>;
