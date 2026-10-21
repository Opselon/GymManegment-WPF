using GymPro.Application.Abstractions;
using GymPro.Domain.Common;
using GymPro.Domain.Users;

namespace GymPro.Application.Users.Commands.AuthenticateUser;

/// <summary>
/// Authenticates a user against the stored password hash. Returns the user's
/// identity on success; never returns the hash.
/// </summary>
public sealed record AuthenticateUserCommand(
    string Username,
    string Password) : ICommand<Result<AuthenticatedUser>>;
