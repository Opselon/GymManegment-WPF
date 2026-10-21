using System.Threading;
using System;
using GymPro.Application.Abstractions;
using GymPro.Application.Memberships.Commands.RegisterMember;
using GymPro.Application.Tests.Fakes;
using GymPro.Domain.Common;
using GymPro.Domain.Memberships;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace GymPro.Application.Tests;

public sealed class RequestDispatcherTests
{
    private static ServiceProvider BuildServices()
    {
        var services = new ServiceCollection();
        var repo = new FakeMembershipRepository();
        services.AddSingleton<IMembershipWriteRepository>(repo);
        services.AddSingleton<IUnitOfWork, FakeUnitOfWork>();
        services.AddSingleton<TimeProvider>(new FixedClock(2024, 3, 31));
        services.AddScoped<IRequestHandler<RegisterMemberCommand, Result<int>>, RegisterMemberHandler>();
        services.AddScoped<IRequestDispatcher, RequestDispatcher>();
        return services.BuildServiceProvider();
    }

    [Fact]
    public async Task A_command_is_routed_to_its_handler()
    {
        await using var provider = BuildServices();
        using var scope = provider.CreateScope();
        var dispatcher = scope.ServiceProvider.GetRequiredService<IRequestDispatcher>();
        var command = new RegisterMemberCommand(
            "Yara Kiani", (byte)Gender.Female, "1234567890", null,
            StartYear: 1403, StartMonth: 1, StartDay: 1, DurationMonths: 3, MonthlyFeeRial: 100m);

        var result = await dispatcher.Send(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task An_unregistered_request_fails_loudly_instead_of_returning_default()
    {
        await using var provider = BuildServices();
        var dispatcher = provider.GetRequiredService<IRequestDispatcher>();

        await Should.ThrowAsync<InvalidOperationException>(async () =>
            await dispatcher.Send(new UnregisteredRequest(), CancellationToken.None));
    }

    private sealed record UnregisteredRequest : IQuery<string>;
}
