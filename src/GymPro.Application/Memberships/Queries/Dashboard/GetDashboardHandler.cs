using GymPro.Application.Abstractions;
using GymPro.Domain.Memberships;
using GymPro.Domain.ValueObjects;

namespace GymPro.Application.Memberships.Queries.Dashboard;

/// <summary>Computes the dashboard numbers from the membership aggregate.</summary>
public sealed class GetDashboardHandler(
    IMembershipReadRepository members,
    TimeProvider clock)
    : IQueryHandler<GetDashboardQuery, DashboardSummary>
{
    public async Task<DashboardSummary> Handle(
        GetDashboardQuery request,
        CancellationToken cancellationToken)
    {
        var today = JalaliDate.Today(clock);
        var all = await members.ListAsync(cancellationToken);

        var active = all.Where(m => !m.IsExpired(today) && m.Status == MemberStatus.Active).ToList();
        var endOfMonth = today.EndOfMonth;
        var expiring = all.Where(m => !m.IsExpired(today) && m.EndOfCoverage >= today && m.EndOfCoverage <= endOfMonth).ToList();
        var outstanding = all.Sum(m => m.OutstandingBalance.RialAmount);
        var collected = all.SelectMany(m => m.Payments)
            .Where(p => p.PaidOn >= today.StartOfMonth)
            .Sum(p => p.Amount.RialAmount);

        return new DashboardSummary(
            TotalMembers: all.Count,
            ActiveMembers: active.Count,
            ExpiringThisMonth: expiring.Count,
            OutstandingTotalRial: outstanding,
            CollectedThisMonthRial: collected);
    }
}
