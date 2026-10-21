using System.Windows.Controls;
using System.Windows.Input;
using GymPro.Presentation.ViewModels;

namespace GymPro.Presentation.Views;

public partial class LoginView : UserControl
{
    public LoginView(LoginViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        Loaded += (_, _) => UsernameBox.Focus();

        // Clearing the box on a failed attempt is the view-model's job, but it
        // only clears the bound value; the PasswordBox keeps its own text.
        viewModel.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(LoginViewModel.Password)
                && string.IsNullOrEmpty(viewModel.Password))
            {
                PasswordInput.Clear();
            }
        };
    }

    private void OnPasswordChanged(object sender, System.Windows.RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel vm && sender is PasswordBox box)
        {
            vm.Password = box.Password;
        }
    }

    /// <summary>
    /// Enter submits from the password box, which is where a person presses it
    /// after typing. PasswordBox does not treat Enter as a default-button click
    /// on its own, so without this the key does nothing.
    /// </summary>
    private void OnPasswordKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && DataContext is LoginViewModel vm)
        {
            if (vm.SignInCommand.CanExecute(null))
            {
                vm.SignInCommand.Execute(null);
                e.Handled = true;
            }
        }
    }
}
