using GymPro.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GymPro.Persistence;

/// <summary>
/// Commits the transaction and dispatches the domain events raised by the
/// aggregates in it. Events fire only after a successful save, so an event
/// never describes a change that was rolled back.
/// </summary>
public sealed class EfUnitOfWork(GymDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await context.SaveChangesAsync(cancellationToken);
}
