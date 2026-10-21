using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymPro.Application.Abstractions;
using GymPro.Application.Memberships.Commands.RegisterMember;
using GymPro.Application.Memberships.Queries.ListMembers;

namespace GymPro.Presentation.ViewModels;

public sealed partial class MembersViewModel(IRequestDispatcher dispatcher) : ViewModelBase
{
    private readonly ObservableCollection<MemberListItem> _members = [];

    public ObservableCollection<MemberListItem> Members => _members;

    [ObservableProperty]
    private string? _searchText;

    [ObservableProperty]
    private string? _nationalCode;

    [ObservableProperty]
    private string? _fullName;

    [ObservableProperty]
    private int _durationMonths = 1;

    [ObservableProperty]
    private decimal _monthlyFeeRial;

    [ObservableProperty]
    private bool _canRegister;

    [RelayCommand]
    public async Task RefreshAsync()
    {
        ClearError();
        IsBusy = true;
        try
        {
            var results = await dispatcher.Send(new ListMembersQuery(SearchText, null));
            _members.Clear();
            foreach (var item in results)
            {
                _members.Add(item);
            }

            Title = "Members";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand(CanExecute = nameof(CanRegister))]
    private async Task RegisterAsync()
    {
        ClearError();
        IsBusy = true;
        try
        {
            var result = await dispatcher.Send(new RegisterMemberCommand(
                FullName ?? string.Empty,
                Gender: 0,
                NationalCode ?? string.Empty,
                PhoneNumber: null,
                StartYear: 1404,
                StartMonth: 1,
                StartDay: 1,
                DurationMonths: DurationMonths,
                MonthlyFeeRial: MonthlyFeeRial));

            if (result.IsFailure)
            {
                ErrorMessage = result.Error.Message;
                return;
            }

            FullName = null;
            NationalCode = null;
            await RefreshAsync();
        }
        finally
        {
            IsBusy = false;
        }
    }

    partial void OnFullNameChanged(string? value) => ReevaluateCanRegister();

    partial void OnNationalCodeChanged(string? value) => ReevaluateCanRegister();

    private void ReevaluateCanRegister()
        => CanRegister = !string.IsNullOrWhiteSpace(FullName)
            && !string.IsNullOrWhiteSpace(NationalCode)
            && NationalCode!.Length == 10;
}
