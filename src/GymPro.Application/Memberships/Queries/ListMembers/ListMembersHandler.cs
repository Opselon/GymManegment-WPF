using GymPro.Application.Abstractions;
using GymPro.Domain.Memberships;
using GymPro.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace GymPro.Application.Memberships.Queries.ListMembers;

/// <summary>
/// Lists members as read models. Uses the read repository, which is a
/// projection-shaped query rather than aggregate rehydration.
/// </summary>
public sealed class ListMembersHandler(
    IMembershipReadRepository members,
    TimeProvider clock,
    ILogger<ListMembersHandler> logger)
    : IQueryHandler<ListMembersQuery, IReadOnlyList<MemberListItem>>
{
    public async Task<IReadOnlyList<MemberListItem>> Handle(
        ListMembersQuery request,
        CancellationToken cancellationToken)
    {
        var today = JalaliDate.Today(clock);
        var list = await members.ListAsync(cancellationToken);

        var items = list
            .Select(m => new MemberListItem(
                m.Id,
                m.FullName,
                m.NationalCode ?? string.Empty,
                m.PhoneNumber,
                m.IsExpired(today) ? "expired" : m.Status.ToString().ToLowerInvariant(),
                m.StartDate.ToString(),
                m.EndOfCoverage.ToString(),
                m.OutstandingBalance.RialAmount))
            .Where(i => string.IsNullOrEmpty(request.SearchText)
                || i.FullName.Contains(request.SearchText!, StringComparison.OrdinalIgnoreCase)
                || i.NationalCode.Contains(request.SearchText!, StringComparison.OrdinalIgnoreCase))
            .Where(i => string.IsNullOrEmpty(request.StatusFilter) || i.Status == request.StatusFilter)
            .ToList();

        logger.ListedMembers(items.Count);
        return items;
    }
}
