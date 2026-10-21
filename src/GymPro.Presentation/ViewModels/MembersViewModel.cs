using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymPro.Application.Abstractions;
using GymPro.Application.Memberships.Commands.RegisterMember;
using GymPro.Application.Memberships.Queries.ListMembers;
using GymPro.Domain.ValueObjects;

namespace GymPro.Presentation.ViewModels;

public sealed partial class MembersViewModel : ViewModelBase
{
    private readonly IRequestDispatcher? _dispatcher;
    private readonly TimeProvider _clock;
    private readonly ObservableCollection<MemberListItem> _members = [];

    public MembersViewModel(IRequestDispatcher dispatcher, TimeProvider clock)
    {
        _dispatcher = dispatcher;
        _clock = clock;

        // Default to today so registration does not silently back-date or
        // forward-date a membership, which the previous hard-coded start did.
        var today = JalaliDate.Today(clock);
        _startYear = today.Year;
        _startMonth = today.Month;
        _startDay = today.Day;
    }

    public MembersViewModel() : this(null!, TimeProvider.System) { }

    public ObservableCollection<MemberListItem> Members => _members;

    [ObservableProperty]
    private string? _searchText;

    [ObservableProperty]
    private string? _nationalCode;

    [ObservableProperty]
    private string? _fullName;

    [ObservableProperty]
    private string? _phoneNumber;

    [ObservableProperty]
    private bool _isFemale;

    [ObservableProperty]
    private int _startYear;

    [ObservableProperty]
    private int _startMonth;

    [ObservableProperty]
    private int _startDay;

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

        if (_dispatcher is null)
        {
            return;
        }

        IsBusy = true;
        try
        {
            var results = await _dispatcher.Send(new ListMembersQuery(SearchText, null));
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
            var result = await _dispatcher!.Send(new RegisterMemberCommand(
                FullName ?? string.Empty,
                Gender: (byte)(IsFemale ? 1 : 0),
                NationalCode ?? string.Empty,
                PhoneNumber: PhoneNumber,
                StartYear: StartYear,
                StartMonth: StartMonth,
                StartDay: StartDay,
                DurationMonths: DurationMonths,
                MonthlyFeeRial: MonthlyFeeRial));

            if (result.IsFailure)
            {
                ErrorMessage = result.Error.Message;
                return;
            }

            FullName = null;
            NationalCode = null;
            PhoneNumber = null;
            await RefreshAsync();
        }
        finally
        {
            IsBusy = false;
        }
    }

    partial void OnFullNameChanged(string? value) => ReevaluateCanRegister();

    partial void OnNationalCodeChanged(string? value) => ReevaluateCanRegister();

    /// <summary>
    /// Re-evaluates the guard and tells the generated command it may have changed.
    /// Without the notification the button stays disabled even once the form is
    /// valid, because CanRegister alone does not re-query the command.
    /// </summary>
    private void ReevaluateCanRegister()
    {
        CanRegister = !string.IsNullOrWhiteSpace(FullName)
            && NationalCode?.Length == 10;

        RegisterCommand.NotifyCanExecuteChanged();
    }
}
