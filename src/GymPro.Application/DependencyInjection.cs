using FluentValidation;
using GymPro.Application.Abstractions;
using GymPro.Domain.Common;
using GymPro.Application.Memberships.Commands.ChargeMembership;
using GymPro.Application.Memberships.Commands.RegisterMember;
using GymPro.Application.Memberships.Queries.Dashboard;
using GymPro.Application.Memberships.Queries.ListMembers;
using GymPro.Application.Users.Commands.AuthenticateUser;
using Microsoft.Extensions.DependencyInjection;

namespace GymPro.Application;

public static class DependencyInjection
{
    // <summary>
    // Registers the CQRS handlers and pipeline behaviors. Handlers are added
    // by their closed generic interface so the dispatcher can resolve them
    // directly from the container.
    // </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);

        // Dispatch infrastructure
        services.AddSingleton<IRequestDispatcher, RequestDispatcher>();
        services.AddSingleton<IRequestPipelineBehavior, ValidationPipelineBehavior>();

        // Request handlers, keyed by closed generic type.
        AddHandler<RegisterMemberCommand, Result<int>, RegisterMemberHandler>(services);
        AddHandler<ChargeMembershipCommand, Result<ChargeResult>, ChargeMembershipHandler>(services);
        AddHandler<AuthenticateUserCommand, Result<AuthenticatedUser>, AuthenticateUserHandler>(services);
        AddHandler<ListMembersQuery, IReadOnlyList<MemberListItem>, ListMembersHandler>(services);
        AddHandler<GetDashboardQuery, DashboardSummary, GetDashboardHandler>(services);

        // Validators
        AddValidator<RegisterMemberCommand, RegisterMemberCommandValidator>(services);

        return services;
    }

    private static void AddHandler<TRequest, TResponse, THandler>(IServiceCollection services)
        where TRequest : IRequest<TResponse>
        where TResponse : notnull
        where THandler : class, IRequestHandler<TRequest, TResponse>
        => services.AddSingleton<IRequestHandler<TRequest, TResponse>, THandler>();

    private static void AddValidator<TRequest, TValidator>(IServiceCollection services)
        where TValidator : class, IValidator<TRequest>
        => services.AddSingleton<IValidator<TRequest>, TValidator>();
}
