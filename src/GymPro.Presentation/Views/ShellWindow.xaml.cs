using System.Windows;
using System.Windows.Controls;
using GymPro.Presentation.Navigation;
using GymPro.Presentation.ViewModels;

namespace GymPro.Presentation.Views;

public partial class ShellWindow : Window
{
    private readonly INavigationService _navigation;

    public ShellWindow(INavigationService navigation, LoginView login)
    {
        InitializeComponent();

        _navigation = navigation;

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
            SignedInAs.Text = $"{user.DisplayName} ({user.Role})";
        }

        MenuBar.Visibility = Visibility.Visible;
        Navigate<DashboardView>();
    }

    private void OnDashboardClick(object sender, RoutedEventArgs e) => Navigate<DashboardView>();

    private void OnMembersClick(object sender, RoutedEventArgs e) => Navigate<MembersView>();

    private void OnAttendanceClick(object sender, RoutedEventArgs e) => Navigate<AttendanceView>();

    private void OnReportsClick(object sender, RoutedEventArgs e) => Navigate<ReportsView>();

    /// <summary>
    /// Navigates, then runs the screen's initial load so a screen is never
    /// displayed empty when data is already available.
    /// </summary>
    private void Navigate<TView>() where TView : UserControl
    {
        _navigation.NavigateTo<TView>();

        if (NavHost.Content is not FrameworkElement { DataContext: { } viewModel })
        {
            return;
        }

        // Screens name their entry point either LoadAsync or RefreshAsync; both are
        // parameterless commands, so a single lookup covers every screen.
        var entryPoint = viewModel.GetType().GetMethod("LoadAsync")
            ?? viewModel.GetType().GetMethod("RefreshAsync");

        if (entryPoint?.Invoke(viewModel, null) is Task work)
        {
            _ = ObserveAsync(work);
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
