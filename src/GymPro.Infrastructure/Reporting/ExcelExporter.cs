using ClosedXML.Excel;
using GymPro.Application.Abstractions;

namespace GymPro.Infrastructure.Reporting;

/// <summary>
/// Exports tabular data to Excel. The original application printed raw WPF
/// DataGrid content; this produces a real spreadsheet the gym can re-use.
/// </summary>
public sealed class ExcelExporter : IExcelExporter
{
    /// <summary>
    /// Writes rows to a new worksheet named <paramref name="sheetName"/> and
    /// returns the workbook as a byte array.
    /// </summary>
    public byte[] Export(string sheetName, string[] headers, IReadOnlyList<string[]> rows)
    {
        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add(sheetName);
        ws.Cell(1, 1).Value = "Row";
        for (var h = 0; h < headers.Length; h++)
            ws.Cell(1, h + 2).Value = headers[h];

        for (var r = 0; r < rows.Count; r++)
        {
            ws.Cell(r + 2, 1).Value = r + 1;
            for (var c = 0; c < rows[r].Length; c++)
                ws.Cell(r + 2, c + 2).Value = rows[r][c];
        }

        ws.Columns().AdjustToContents();
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
