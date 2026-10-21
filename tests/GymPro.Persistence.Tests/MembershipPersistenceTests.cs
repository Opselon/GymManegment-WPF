using GymPro.Domain.Memberships;
using GymPro.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace GymPro.Persistence.Tests;

public sealed class MembershipPersistenceTests
{
    private static readonly TimeProvider Clock = TimeProvider.System;

    private static Membership NewMember(string nationalCode = "1234567890")
        => Membership.RegisterNew(
            "Yara Kiani", Gender.Female, JalaliDate.Create(1403, 1, 1), 3,
            Money.FromRial(900_000_000m), nationalCode, "0215551111", Clock);

    [Fact]
    public async Task A_membership_round_trips_through_the_database()
    {
        await using var db = new SqliteHarness();
        db.Context.Memberships.Add(NewMember());
        await db.Context.SaveChangesAsync(CancellationToken.None);

        await using var reader = db.NewContext();
        var loaded = await reader.Memberships.SingleAsync(CancellationToken.None);

        loaded.FullName.ShouldBe("Yara Kiani");
        loaded.NationalCode.ShouldBe("1234567890");
        loaded.Status.ShouldBe(MemberStatus.Active);
        loaded.DurationMonths.ShouldBe(3);
    }

    [Fact]
    public async Task The_Jalali_value_object_survives_the_round_trip_unchanged()
    {
        await using var db = new SqliteHarness();
        var original = JalaliDate.Create(1403, 7, 15);
        var member = NewMember();
        member.StartDate = original;
        db.Context.Memberships.Add(member);
        await db.Context.SaveChangesAsync(CancellationToken.None);

        await using var reader = db.NewContext();
        var loaded = await reader.Memberships.SingleAsync(CancellationToken.None);

        loaded.StartDate.ShouldBe(original);
        loaded.StartDate.Year.ShouldBe(1403);
        loaded.StartDate.Month.ShouldBe(7);
        loaded.StartDate.Day.ShouldBe(15);
    }

    [Fact]
    public async Task Money_is_stored_exactly()
    {
        await using var db = new SqliteHarness();
        db.Context.Memberships.Add(NewMember());
        await db.Context.SaveChangesAsync(CancellationToken.None);

        await using var reader = db.NewContext();
        var loaded = await reader.Memberships.SingleAsync(CancellationToken.None);

        loaded.MonthlyFee.RialAmount.ShouldBe(900_000_000m);
    }

    [Fact]
    public async Task Payments_persist_as_a_child_collection()
    {
        await using var db = new SqliteHarness();
        var member = NewMember();
        member.RecordPayment(Money.FromRial(100_000_000m), JalaliDate.Create(1403, 1, 10), Clock);
        member.RecordPayment(Money.FromRial(50_000_000m), JalaliDate.Create(1403, 2, 10), Clock);
        db.Context.Memberships.Add(member);
        await db.Context.SaveChangesAsync(CancellationToken.None);

        await using var reader = db.NewContext();
        var loaded = await reader.Memberships.Include(m => m.Payments).SingleAsync(CancellationToken.None);

        loaded.Payments.Count.ShouldBe(2);
        loaded.OutstandingBalance.RialAmount.ShouldBe(2_700_000_000m - 150_000_000m);
    }

    [Fact]
    public async Task A_duplicate_national_code_is_rejected_by_the_database()
    {
        await using var db = new SqliteHarness();
        db.Context.Memberships.Add(NewMember("1111111111"));
        await db.Context.SaveChangesAsync(CancellationToken.None);

        await using var second = db.NewContext();
        second.Memberships.Add(NewMember("1111111111"));

        await Should.ThrowAsync<DbUpdateException>(
            async () => await second.SaveChangesAsync(CancellationToken.None));
    }

    [Fact]
    public async Task Two_different_national_codes_are_allowed()
    {
        await using var db = new SqliteHarness();
        db.Context.Memberships.Add(NewMember("1111111111"));
        db.Context.Memberships.Add(NewMember("2222222222"));

        await db.Context.SaveChangesAsync(CancellationToken.None);

        (await db.Context.Memberships.CountAsync(CancellationToken.None)).ShouldBe(2);
    }

    [Fact]
    public async Task Deleting_a_membership_cascades_to_its_payments()
    {
        await using var db = new SqliteHarness();
        var member = NewMember();
        member.RecordPayment(Money.FromRial(100m), JalaliDate.Create(1403, 1, 10), Clock);
        db.Context.Memberships.Add(member);
        await db.Context.SaveChangesAsync(CancellationToken.None);

        db.Context.Memberships.Remove(member);
        await db.Context.SaveChangesAsync(CancellationToken.None);

        await using var reader = db.NewContext();
        (await reader.Payments.CountAsync(CancellationToken.None)).ShouldBe(0);
    }

    [Fact]
    public async Task The_schema_creates_without_errors()
    {
        await using var db = new SqliteHarness();

        (await db.Context.Database.CanConnectAsync(CancellationToken.None)).ShouldBeTrue();

        var tables = db.Context.Model.GetEntityTypes().Select(t => t.GetTableName()).ToList();
        tables.ShouldContain("Memberships");
        tables.ShouldContain("Payments");
        tables.ShouldContain("Users");
        tables.ShouldContain("CheckIns");
    }
}
