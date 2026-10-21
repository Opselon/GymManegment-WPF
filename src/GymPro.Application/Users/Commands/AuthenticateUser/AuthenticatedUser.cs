using GymPro.Application.Abstractions;
using GymPro.Domain.Common;
using GymPro.Domain.Users;

namespace GymPro.Application.Users.Commands.AuthenticateUser;

/// <summary>
/// Read model returned after successful authentication. Carries no secrets.
/// </summary>
public sealed record AuthenticatedUser(int Id, string Username, string DisplayName, UserRole Role);
