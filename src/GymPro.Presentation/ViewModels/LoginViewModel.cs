using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymPro.Application.Abstractions;
using GymPro.Application.Users.Commands.AuthenticateUser;
using GymPro.Domain.Common;

namespace GymPro.Presentation.ViewModels;

public sealed partial class LoginViewModel(IRequestDispatcher dispatcher) : ViewModelBase
{
    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    public AuthenticatedUser? AuthenticatedUser { get; private set; }

    /// <summary>Raised when authentication succeeds so the shell can navigate.</summary>
    public event EventHandler? SignedIn;

    [RelayCommand]
    private async Task SignInAsync()
    {
        ClearError();
        IsBusy = true;
        try
        {
            var result = await dispatcher.Send(new AuthenticateUserCommand(Username, Password));
            if (result.IsFailure)
            {
                ErrorMessage = result.Error.Message;
                return;
            }

            AuthenticatedUser = result.Value;
            Password = string.Empty;
            SignedIn?.Invoke(this, EventArgs.Empty);
        }
        finally
        {
            IsBusy = false;
        }
    }
}
