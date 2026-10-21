using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymPro.Application.Abstractions;
using GymPro.Application.Attendance.Queries.ListCheckIns;
using GymPro.Application.Memberships.Queries.Dashboard;

namespace GymPro.Presentation.ViewModels;

public sealed partial class DashboardViewModel(IRequestDispatcher dispatcher) : ViewModelBase
{
    private readonly ObservableCollection<CheckInListItem> _recentCheckIns = [];

    public ObservableCollection<CheckInListItem> RecentCheckIns => _recentCheckIns;

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

    /// <summary>
    /// Debt per active member. Two cards in a three-column row leave a hole, and
    /// this is the number that tells the desk whether the debt is spread thin or
    /// concentrated in one or two accounts.
    /// </summary>
    [ObservableProperty]
    private decimal _averageOutstandingRial;

    public DashboardViewModel() : this(null!) { }

    /// <summary>
    /// Loads the summary and the recent check-ins together. Splitting them would
    /// mean the dashboard opens with an empty activity list until somebody
    /// presses refresh, which reads as broken rather than empty.
    /// </summary>
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
            AverageOutstandingRial = summary.ActiveMembers == 0
                ? 0
                : Math.Round(summary.OutstandingTotalRial / summary.ActiveMembers, MidpointRounding.ToEven);
            Title = "Dashboard";

            var checkIns = await dispatcher.Send(new ListCheckInsQuery(MaxResults: 8));
            _recentCheckIns.Clear();
            foreach (var item in checkIns)
            {
                _recentCheckIns.Add(item);
            }
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
