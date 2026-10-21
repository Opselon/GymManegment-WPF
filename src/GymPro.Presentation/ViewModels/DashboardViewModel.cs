using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymPro.Application.Abstractions;
using GymPro.Application.Memberships.Queries.Dashboard;

namespace GymPro.Presentation.ViewModels;

public sealed partial class DashboardViewModel(IRequestDispatcher dispatcher) : ViewModelBase
{
    [ObservableProperty]
    private int _totalMembers;

    [ObservableProperty]
    private int _activeMembers;

    [ObservableProperty]
    private int _expiringThisMonth;

    [ObservableProperty]
    private decimal _outstandingTotalRial;

    [ObservableProperty]
    private decimal _collectedThisMonthRial;

    public DashboardViewModel() : this(null!) { }

    [RelayCommand]
    public async Task LoadAsync()
    {
        ClearError();
        IsBusy = true;
        try
        {
            var summary = await dispatcher.Send(new GetDashboardQuery());
            TotalMembers = summary.TotalMembers;
            ActiveMembers = summary.ActiveMembers;
            ExpiringThisMonth = summary.ExpiringThisMonth;
            OutstandingTotalRial = summary.OutstandingTotalRial;
            CollectedThisMonthRial = summary.CollectedThisMonthRial;
            Title = "Dashboard";
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
