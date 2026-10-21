using GymPro.Application.Abstractions;
using GymPro.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace GymPro.Persistence.Repositories;

public sealed class UserRepository(GymDbContext context) : IUserReadRepository, IUserWriteRepository
{
    public void Add(User user) => context.Users.Add(user);

    public void Remove(User aggregate) => context.Users.Remove(aggregate);

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        => await context.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public async Task<IReadOnlyList<User>> ListAsync(CancellationToken cancellationToken = default)
        => await context.Users.AsNoTracking().OrderBy(u => u.Username).ToListAsync(cancellationToken);
}
