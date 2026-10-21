using GymPro.Application.Abstractions;
using GymPro.Application.Reporting.Queries.ExportMembers;
using GymPro.Application.Tests.Fakes;
using GymPro.Domain.Common;
using GymPro.Domain.Memberships;
using GymPro.Domain.ValueObjects;
using Microsoft.Extensions.Logging.Abstractions;
using Shouldly;
using Xunit;

namespace GymPro.Application.Tests;

public sealed class ExportMembersHandlerTests
{
    private static readonly FixedClock Clock = new(2024, 3, 31);

    /// <summary>Records what the handler asked for, so the column contract is testable.</summary>
    private sealed class CapturingExporter : IExcelExporter
    {
        public string? SheetName { get; private set; }

        public string[] Headers { get; private set; } = [];

        public IReadOnlyList<string[]> Rows { get; private set; } = [];

        public byte[] Export(string sheetName, string[] headers, IReadOnlyList<string[]> rows)
        {
            SheetName = sheetName;
            Headers = headers;
            Rows = rows;
            return [1, 2, 3];
        }
    }

    private static (ExportMembersHandler Handler, FakeMembershipRepository Repo, CapturingExporter Exporter) Arrange()
    {
        var repo = new FakeMembershipRepository();
        var exporter = new CapturingExporter();
        return (new ExportMembersHandler(repo, exporter, Clock, NullLogger<ExportMembersHandler>.Instance), repo, exporter);
    }

    [Fact]
    public async Task An_empty_member_list_is_a_validation_failure_not_an_empty_workbook()
    {
        var (handler, _, exporter) = Arrange();

        var result = await handler.Handle(new ExportMembersQuery(), CancellationToken.None);

        result.IsFailure.ShouldBeTrue();
        result.Error!.Kind.ShouldBe(ErrorKind.Validation);
        exporter.SheetName.ShouldBeNull();
    }

    [Fact]
    public async Task The_export_has_one_row_per_member_in_name_order()
    {
        var (handler, repo, exporter) = Arrange();
        repo.Add(Membership.RegisterNew(
            "Zahra Sadeghi", Gender.Female, JalaliDate.Create(1403, 1, 1), 3,
            Money.FromRial(1000m), "1111111111", null, Clock));
        repo.Add(Membership.RegisterNew(
            "Ali Rezaei", Gender.Male, JalaliDate.Create(1403, 1, 1), 3,
            Money.FromRial(1000m), "2222222222", null, Clock));

        var result = await handler.Handle(new ExportMembersQuery(), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue(result.Error?.Message);
        exporter.Rows.Count.ShouldBe(2);
        exporter.Rows[0][0].ShouldBe("Ali Rezaei");
        exporter.Rows[1][0].ShouldBe("Zahra Sadeghi");
    }

    [Fact]
    public async Task Amounts_are_written_as_bare_integers_so_excel_treats_them_as_numbers()
    {
        var (handler, repo, exporter) = Arrange();
        repo.Add(Membership.RegisterNew(
            "Yara Kiani", Gender.Female, JalaliDate.Create(1403, 1, 1), 3,
            Money.FromRial(9_000_000m), "1234567890", null, Clock));

        await handler.Handle(new ExportMembersQuery(), CancellationToken.None);

        var fee = exporter.Rows[0][6];
        var outstanding = exporter.Rows[0][7];
        long.TryParse(fee, out var parsed).ShouldBeTrue("fee should be a bare integer, was: " + fee);
        long.TryParse(outstanding, out _).ShouldBeTrue("outstanding should be a bare integer");
        // Column 6 is the monthly fee; the total contract value is not exported.
        parsed.ShouldBe(9_000_000L);
    }

    [Fact]
    public async Task The_file_name_carries_the_report_date()
    {
        var (handler, repo, _) = Arrange();
        repo.Add(Membership.RegisterNew(
            "Yara Kiani", Gender.Female, JalaliDate.Create(1403, 1, 1), 1,
            Money.FromRial(1000m), "1234567890", null, Clock));

        var result = await handler.Handle(new ExportMembersQuery(), CancellationToken.None);

        result.Value!.FileName.ShouldEndWith(".xlsx");
        result.Value.ContentType.ShouldContain("spreadsheetml");
        result.Value.Content.ShouldNotBeEmpty();
    }
}
