using System.Windows;
using System.Windows.Controls;
using GymPro.Presentation.Navigation;

namespace GymPro.Presentation.Views;

public partial class ShellWindow : Window
{
    public ShellWindow(INavigationService navigation, LoginView login)
    {
        InitializeComponent();

        navigation.RegisterHost(NavHost);
        NavHost.Content = login;
    }
}
