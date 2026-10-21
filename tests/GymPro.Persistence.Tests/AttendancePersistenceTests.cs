using GymPro.Application.Abstractions;
using GymPro.Domain.Attendance;
using GymPro.Domain.Memberships;
using GymPro.Domain.ValueObjects;
using GymPro.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace GymPro.Persistence.Tests;

/// <summary>
/// The attendance and measurement stores, exercised against real SQLite. This is
/// where the projection bugs live: an EF query that translates fine in memory can
/// still fail once it becomes SQL.
/// </summary>
public sealed class AttendancePersistenceTests
{
    private static readonly TimeProvider Clock = TimeProvider.System;

    private static Membership SeedMember(SqliteHarness db, string name = "Yara Kiani")
    {
        var member = Membership.RegisterNew(
            name, Gender.Female, JalaliDate.Create(1403, 1, 1), 6,
            Money.FromRial(1000m), "1234567890", null, Clock);
        db.Context.Memberships.Add(member);
        db.Context.SaveChanges();
        return member;
    }

    [Fact]
    public async Task A_check_in_persists_and_counts_for_its_day()
    {
        await using var db = new SqliteHarness();
        var member = SeedMember(db);
        var day = JalaliDate.Create(1403, 2, 1);

        var write = new CheckInWriteRepository(db.Context);
        write.Add(CheckIn.Record(member, day, Clock));
        await db.Context.SaveChangesAsync(CancellationToken.None);

        (await write.CountForDateAsync(day, CancellationToken.None)).ShouldBe(1);
        (await write.CountForDateAsync(day.AddDays(1), CancellationToken.None)).ShouldBe(0);
    }

    [Fact]
    public async Task The_recent_list_joins_the_member_name()
    {
        await using var db = new SqliteHarness();
        var member = SeedMember(db, "Reza Ahmadi");

        var write = new CheckInWriteRepository(db.Context);
        write.Add(CheckIn.Record(member, JalaliDate.Create(1403, 2, 1), Clock));
        await db.Context.SaveChangesAsync(CancellationToken.None);

        var read = new CheckInReadRepository(db.Context);
        var rows = await read.ListRecentAsync(50, CancellationToken.None);

        rows.ShouldHaveSingleItem().MemberName.ShouldBe("Reza Ahmadi");
    }

    [Fact]
    public async Task The_recent_list_is_newest_first_and_honours_the_limit()
    {
        await using var db = new SqliteHarness();
        var member = SeedMember(db);

        var write = new CheckInWriteRepository(db.Context);
        write.Add(CheckIn.Record(member, JalaliDate.Create(1403, 2, 1), Clock));
        write.Add(CheckIn.Record(member, JalaliDate.Create(1403, 2, 2), Clock));
        write.Add(CheckIn.Record(member, JalaliDate.Create(1403, 2, 3), Clock));
        await db.Context.SaveChangesAsync(CancellationToken.None);

        var read = new CheckInReadRepository(db.Context);
        var rows = await read.ListRecentAsync(2, CancellationToken.None);

        rows.Count.ShouldBe(2);
        rows[0].CheckedInOn.ShouldBe(JalaliDate.Create(1403, 2, 3).ToString());
    }

    [Fact]
    public async Task A_measurement_persists_with_its_nullable_columns()
    {
        await using var db = new SqliteHarness();
        var member = SeedMember(db);

        var write = new MeasurementWriteRepository(db.Context);
        write.Add(BodyMeasurement.Record(member.Id, JalaliDate.Create(1403, 2, 10), weightKg: 71, waistCm: 84));
        await db.Context.SaveChangesAsync(CancellationToken.None);

        var read = new MeasurementReadRepository(db.Context);
        var rows = await read.ListForMemberAsync(member.Id, CancellationToken.None);

        var row = rows.ShouldHaveSingleItem();
        row.WeightKg.ShouldBe(71);
        row.WaistCm.ShouldBe(84);
        row.ChestCm.ShouldBeNull();
        row.MeasuredOn.ShouldBe(JalaliDate.Create(1403, 2, 10).ToString());
    }

    [Fact]
    public async Task The_measurement_history_is_oldest_first()
    {
        await using var db = new SqliteHarness();
        var member = SeedMember(db);

        var write = new MeasurementWriteRepository(db.Context);
        write.Add(BodyMeasurement.Record(member.Id, JalaliDate.Create(1403, 3, 1), weightKg: 70));
        write.Add(BodyMeasurement.Record(member.Id, JalaliDate.Create(1403, 1, 1), weightKg: 75));
        write.Add(BodyMeasurement.Record(member.Id, JalaliDate.Create(1403, 2, 1), weightKg: 72));
        await db.Context.SaveChangesAsync(CancellationToken.None);

        var read = new MeasurementReadRepository(db.Context);
        var rows = await read.ListForMemberAsync(member.Id, CancellationToken.None);

        rows.Select(r => r.WeightKg).ShouldBe([75, 72, 70]);
    }

    [Fact]
    public async Task Measurements_do_not_leak_between_members()
    {
        await using var db = new SqliteHarness();
        var first = SeedMember(db);

        var second = Membership.RegisterNew(
            "Reza Ahmadi", Gender.Male, JalaliDate.Create(1403, 1, 1), 6,
            Money.FromRial(1000m), "0987654321", null, Clock);
        db.Context.Memberships.Add(second);
        await db.Context.SaveChangesAsync(CancellationToken.None);

        var write = new MeasurementWriteRepository(db.Context);
        write.Add(BodyMeasurement.Record(first.Id, JalaliDate.Create(1403, 2, 1), weightKg: 70));
        write.Add(BodyMeasurement.Record(second.Id, JalaliDate.Create(1403, 2, 1), weightKg: 90));
        await db.Context.SaveChangesAsync(CancellationToken.None);

        var read = new MeasurementReadRepository(db.Context);

        (await read.ListForMemberAsync(first.Id, CancellationToken.None))
            .ShouldHaveSingleItem().WeightKg.ShouldBe(70);
        (await read.ListForMemberAsync(second.Id, CancellationToken.None))
            .ShouldHaveSingleItem().WeightKg.ShouldBe(90);
    }
}
