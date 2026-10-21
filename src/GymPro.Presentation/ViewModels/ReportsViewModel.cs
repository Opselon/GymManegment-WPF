using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymPro.Application.Abstractions;
using GymPro.Application.Reporting.Queries.ExportMembers;

namespace GymPro.Presentation.ViewModels;

/// <summary>
/// Reporting screen. Generating the workbook is a use case; saving the file is a
/// UI concern, so the view-model owns only the save dialog.
/// </summary>
public sealed partial class ReportsViewModel(IRequestDispatcher dispatcher) : ViewModelBase
{
    public ReportsViewModel() : this(null!) { }

    [ObservableProperty]
    private string? _statusMessage;

    /// <summary>Where the last export landed, shown so the operator can find it.</summary>
    [ObservableProperty]
    private string? _lastExportPath;

    [RelayCommand]
    public async Task ExportMembersAsync()
    {
        ClearError();
        StatusMessage = null;
        IsBusy = true;
        try
        {
            var result = await dispatcher.Send(new ExportMembersQuery());

            if (result.IsFailure)
            {
                ErrorMessage = result.Error.Message;
                return;
            }

            var file = result.Value!;
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = file.FileName,
                Filter = "Excel workbook (*.xlsx)|*.xlsx",
                DefaultExt = ".xlsx",
                AddExtension = true,
            };

            if (dialog.ShowDialog() != true)
            {
                StatusMessage = "ذخیره لغو شد.";
                return;
            }

            await File.WriteAllBytesAsync(dialog.FileName, file.Content);

            LastExportPath = dialog.FileName;
            StatusMessage = $"گزارش ذخیره شد ({file.Content.Length / 1024} کیلوبایت).";
        }
        catch (IOException ex)
        {
            // A file locked by Excel is a routine failure, not a crash.
            ErrorMessage = $"نوشتن فایل ناموفق بود: {ex.Message}";
        }
        catch (UnauthorizedAccessException ex)
        {
            ErrorMessage = $"دسترسی به مسیر وجود ندارد: {ex.Message}";
        }
        catch (InvalidOperationException ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
