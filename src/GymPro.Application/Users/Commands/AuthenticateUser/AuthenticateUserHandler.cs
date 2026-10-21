using GymPro.Application.Abstractions;
using GymPro.Domain.Common;

namespace GymPro.Application.Users.Commands.AuthenticateUser;

/// <summary>
/// Validates credentials against the stored hash. The hash lives in the User
/// aggregate; comparison happens through the hasher port, so the application
/// layer never sees an algorithm.
/// </summary>
public sealed class AuthenticateUserHandler(
    IUserReadRepository users,
    IPasswordHasher hasher)
    : ICommandHandler<AuthenticateUserCommand, Result<AuthenticatedUser>>
{
    public async Task<Result<AuthenticatedUser>> Handle(
        AuthenticateUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await users.GetByUsernameAsync(request.Username, cancellationToken);
        if (user is null || !user.IsActive)
        {
            // Deliberately the same message for both failures: revealing which
            // one failed leaks which usernames exist.
            return Result.Failure<AuthenticatedUser>(DomainError.Validation("credentials", "Username or password is incorrect."));
        }

        if (!hasher.Verify(request.Password, user.PasswordHash))
        {
            return Result.Failure<AuthenticatedUser>(DomainError.Validation("credentials", "Username or password is incorrect."));
        }

        return Result.Success(new AuthenticatedUser(user.Id, user.Username, user.DisplayName, user.Role));
    }
}
