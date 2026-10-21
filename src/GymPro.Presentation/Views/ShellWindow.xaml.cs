using System.Windows;
using System.Windows.Controls;
using GymPro.Presentation.Navigation;
using GymPro.Presentation.ViewModels;

namespace GymPro.Presentation.Views;

public partial class ShellWindow : Window
{
    private readonly INavigationService _navigation;
    private readonly Dictionary<Type, Button> _navForView;

    public ShellWindow(INavigationService navigation, LoginView login)
    {
        InitializeComponent();

        _navigation = navigation;

        // Which header button represents which screen. Keeping this in one map
        // means adding a screen cannot forget to update the active state.
        _navForView = new Dictionary<Type, Button>
        {
            [typeof(DashboardView)] = NavDashboard,
            [typeof(MembersView)] = NavMembers,
            [typeof(AttendanceView)] = NavAttendance,
            [typeof(MeasurementsView)] = NavMeasurements,
            [typeof(ReportsView)] = NavReports,
            [typeof(BackupView)] = NavBackup,
        };

        navigation.RegisterHost(NavHost);
        NavHost.Content = login;

        // The login screen owns the transition out of itself; the shell only
        // reacts, so the view stays ignorant of the menu. LoginView is transient,
        // so this handler is collected along with it.
        if (login.DataContext is LoginViewModel loginViewModel)
        {
            loginViewModel.SignedIn += OnSignedIn;
        }
    }

    private void OnSignedIn(object? sender, EventArgs e)
    {
        if (sender is LoginViewModel { AuthenticatedUser: { } user })
        {
            SignedInAs.Text = $"{user.DisplayName} · {user.Role}";
        }

        MenuBar.Visibility = Visibility.Visible;
        Navigate<DashboardView>();
    }

    private void OnDashboardClick(object sender, RoutedEventArgs e) => Navigate<DashboardView>();

    private void OnMembersClick(object sender, RoutedEventArgs e) => Navigate<MembersView>();

    private void OnAttendanceClick(object sender, RoutedEventArgs e) => Navigate<AttendanceView>();

    private void OnMeasurementsClick(object sender, RoutedEventArgs e) => Navigate<MeasurementsView>();

    private void OnReportsClick(object sender, RoutedEventArgs e) => Navigate<ReportsView>();

    private void OnBackupClick(object sender, RoutedEventArgs e) => Navigate<BackupView>();

    /// <summary>
    /// Navigates, marks the matching header button as active, then runs the
    /// screen's initial load so a screen is never shown empty when data exists.
    /// </summary>
    private void Navigate<TView>() where TView : UserControl
    {
        _navigation.NavigateTo<TView>();
        MarkActive(typeof(TView));

        if (NavHost.Content is not FrameworkElement { DataContext: { } viewModel })
        {
            return;
        }

        // Screens name their entry point either LoadAsync or RefreshAsync; both are
        // parameterless, so a single lookup covers every screen.
        var entryPoint = viewModel.GetType().GetMethod("LoadAsync")
            ?? viewModel.GetType().GetMethod("RefreshAsync");

        if (entryPoint?.Invoke(viewModel, null) is Task work)
        {
            _ = ObserveAsync(work);
        }
    }

    /// <summary>
    /// The nav style highlights whichever button carries Tag="Active". Clearing
    /// the tag on all of them first is what makes the highlight move rather than
    /// accumulate.
    /// </summary>
    private void MarkActive(Type viewType)
    {
        foreach (var button in _navForView.Values)
        {
            button.Tag = null;
        }

        if (_navForView.TryGetValue(viewType, out var active))
        {
            active.Tag = "Active";
        }
    }

    /// <summary>
    /// Awaits the screen load without blocking navigation. The view-model already
    /// routes failures to its own error surface; this only stops the exception
    /// from being swallowed silently if one escapes.
    /// </summary>
    private async Task ObserveAsync(Task work)
    {
        try
        {
            await work;
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(this, ex.Message, "GymPro", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
