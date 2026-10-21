using CommunityToolkit.Mvvm.ComponentModel;

namespace GymPro.Presentation.ViewModels;

/// <summary>
/// Base for screen view-models: carries the busy flag and error surface that
/// every screen needs, so no screen re-implements them.
/// </summary>
public abstract partial class ViewModelBase : ObservableObject
{
    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string? _errorMessage;

    [ObservableProperty]
    private string _title = string.Empty;

    protected void ClearError() => ErrorMessage = null;
}
