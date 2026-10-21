using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace GymPro.Presentation.Navigation;

/// <summary>
/// Swaps the content of the shell's content host. Screens stay ignorant of each
/// other and of the shell, which is what keeps navigation testable.
/// </summary>
public interface INavigationService
{
    void NavigateTo<TView>() where TView : UserControl;
    void RegisterHost(ContentControl host);
}

public sealed class NavigationService(IServiceProvider services) : INavigationService
{
    private ContentControl? _host;

    public void RegisterHost(ContentControl host) => _host = host;

    public void NavigateTo<TView>() where TView : UserControl
    {
        if (_host is null)
        {
            throw new InvalidOperationException("Navigation host has not been registered.");
        }

        _host.Content = services.GetRequiredService<TView>();
    }
}
