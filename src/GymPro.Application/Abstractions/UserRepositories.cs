using GymPro.Domain.Users;

namespace GymPro.Application.Abstractions;

/// <summary>
/// Read side of the user aggregate.
/// </summary>
public interface IUserReadRepository
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<User>> ListAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Write side of the user aggregate.
/// </summary>
public interface IUserWriteRepository : IWriteRepository<User>;
