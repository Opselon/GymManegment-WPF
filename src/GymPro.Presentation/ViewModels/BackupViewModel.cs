using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymPro.Application.Abstractions;

namespace GymPro.Presentation.ViewModels;

/// <summary>
/// Backup screen. Takes a copy-only backup and prunes old ones, so the operator
/// does not need SSMS to protect the member ledger.
/// </summary>
/// <remarks>
/// There is no domain rule to enforce here, so the screen calls the service
/// directly rather than inventing a command and handler that would only forward
/// the call. When no backup directory is configured the service is absent and
/// the screen disables its buttons instead of failing on click.
/// </remarks>
public sealed partial class BackupViewModel : ViewModelBase
{
    private readonly IDatabaseBackupService? _backupService;

    public BackupViewModel(IDatabaseBackupService? backupService = null)
    {
        _backupService = backupService;
        IsAvailable = backupService is not null;
    }

    public ObservableCollection<BackupFileItem> Backups { get; } = [];

    [ObservableProperty]
    private bool _isAvailable;

    [ObservableProperty]
    private int _keepNewest = 5;

    [ObservableProperty]
    private string? _statusMessage;

    [RelayCommand]
    public Task LoadAsync()
    {
        ClearError();
        RefreshList();
        return Task.CompletedTask;
    }

    [RelayCommand(CanExecute = nameof(IsAvailable))]
    private async Task CreateBackupAsync()
    {
        ClearError();
        StatusMessage = null;
        IsBusy = true;
        try
        {
            var path = await _backupService!.CreateBackupAsync();
            StatusMessage = $"پشتیبان ساخته شد: {Path.GetFileName(path)}";
            RefreshList();
        }
        catch (IOException ex)
        {
            ErrorMessage = $"نوشتن فایل پشتیبان ناموفق بود: {ex.Message}";
        }
        catch (UnauthorizedAccessException ex)
        {
            ErrorMessage = $"دسترسی به مسیر پشتیبان وجود ندارد: {ex.Message}";
        }
        catch (InvalidOperationException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            // A backup failure must never take the application down with it; the
            // ledger is still open and the operator can try again. Report and let
            // WPF keep running.
            ErrorMessage = $"پشتیبان‌گیری ناموفق بود: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand(CanExecute = nameof(IsAvailable))]
    private void PruneBackups()
    {
        ClearError();
        StatusMessage = null;
        try
        {
            var removed = _backupService!.PruneBackups(KeepNewest);
            StatusMessage = removed.Count == 0
                ? "پشتیبان قدیمی‌تری برای حذف وجود نداشت."
                : $"{removed.Count} پشتیبان قدیمی حذف شد.";
            RefreshList();
        }
        catch (IOException ex)
        {
            ErrorMessage = $"حذف پشتیبان ناموفق بود: {ex.Message}";
        }
        catch (UnauthorizedAccessException ex)
        {
            ErrorMessage = $"دسترسی به مسیر پشتیبان وجود ندارد: {ex.Message}";
        }
    }

    /// <summary>
    /// Lists what is actually on disk in the directory the service writes to,
    /// rather than trusting a second configuration value to agree with it.
    /// </summary>
    private void RefreshList()
    {
        Backups.Clear();

        if (_backupService is null)
        {
            return;
        }

        var directory = new DirectoryInfo(_backupService.BackupDirectory);
        if (!directory.Exists)
        {
            return;
        }

        foreach (var file in directory.EnumerateFiles("*.bak")
                     .OrderByDescending(f => f.LastWriteTimeUtc)
                     .Take(50))
        {
            Backups.Add(new BackupFileItem(
                file.Name,
                $"{file.Length / 1024d / 1024d:F1} MB",
                file.LastWriteTimeUtc.ToLocalTime().ToString("yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture)));
        }
    }
}

/// <summary>One backup file, formatted for display.</summary>
public sealed record BackupFileItem(string FileName, string Size, string CreatedAt);
