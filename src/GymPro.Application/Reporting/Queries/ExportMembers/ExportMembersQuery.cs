using GymPro.Application.Abstractions;
using GymPro.Domain.Common;
using GymPro.Domain.Memberships;
using GymPro.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace GymPro.Application.Reporting.Queries.ExportMembers;

/// <summary>
/// Exports the member list as a spreadsheet.
/// </summary>
/// <remarks>
/// The handler owns the column layout and the numbers; the export format is an
/// infrastructure detail behind <see cref="IExcelExporter"/>. Amounts are
/// written as plain rial integers so the columns are numeric in Excel, not text
/// with thousands separators that a formula cannot read.
/// </remarks>
public sealed record ExportMembersQuery : IQuery<Result<ReportFile>>;

/// <summary>A generated report, ready to be saved or shown.</summary>
public sealed record ReportFile(string FileName, string ContentType, byte[] Content);

public sealed class ExportMembersHandler(
    IMembershipReadRepository members,
    IExcelExporter exporter,
    TimeProvider clock,
    ILogger<ExportMembersHandler> logger)
    : IQueryHandler<ExportMembersQuery, Result<ReportFile>>
{
    private static readonly string[] Headers =
    [
        "Name",
        "National code",
        "Phone",
        "Status",
        "Start",
        "End of coverage",
        "Monthly fee (rial)",
        "Outstanding (rial)",
    ];

    public async Task<Result<ReportFile>> Handle(
        ExportMembersQuery request,
        CancellationToken cancellationToken)
    {
        var today = JalaliDate.Today(clock);
        var all = await members.ListAsync(cancellationToken);

        if (all.Count == 0)
        {
            return Result.Failure<ReportFile>(
                DomainError.Validation("members", "There are no members to export."));
        }

        var rows = all
            .OrderBy(m => m.FullName, StringComparer.Ordinal)
            .Select(m => new[]
            {
                m.FullName,
                m.NationalCode ?? string.Empty,
                m.PhoneNumber ?? string.Empty,
                m.IsExpired(today) ? "expired" : m.Status.ToString(),
                m.StartDate.ToString(),
                m.EndOfCoverage.ToString(),
                m.MonthlyFee.RialAmount.ToString(System.Globalization.CultureInfo.InvariantCulture),
                m.OutstandingBalance.RialAmount.ToString(System.Globalization.CultureInfo.InvariantCulture),
            })
            .ToList();

        var content = exporter.Export($"Members {today}", Headers, rows);

        ExportLog.Exported(logger, rows.Count);

        return Result.Success(new ReportFile(
            FileName: $"GymPro-members-{today.Year:D4}{today.Month:D2}{today.Day:D2}.xlsx",
            ContentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            Content: content));
    }
}

/// <summary>Cached log delegates for the reporting handlers.</summary>
internal static class ExportLog
{
    private static readonly Action<ILogger, int, Exception?> ExportedCore =
        LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(2100, nameof(Exported)),
            "Exported {Count} members to a spreadsheet.");

    public static void Exported(this ILogger logger, int count) => ExportedCore(logger, count, null);
}
