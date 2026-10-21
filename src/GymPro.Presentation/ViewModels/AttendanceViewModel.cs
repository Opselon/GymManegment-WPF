using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymPro.Application.Abstractions;
using GymPro.Application.Attendance.Commands.CheckInMember;
using GymPro.Application.Attendance.Queries.ListCheckIns;
using GymPro.Application.Memberships.Queries.ListMembers;

namespace GymPro.Presentation.ViewModels;

/// <summary>
/// The front-desk screen: pick a member, record the arrival, see today's log.
/// </summary>
public sealed partial class AttendanceViewModel(IRequestDispatcher dispatcher) : ViewModelBase
{
    private readonly ObservableCollection<CheckInListItem> _checkIns = [];
    private readonly ObservableCollection<MemberListItem> _members = [];

    public AttendanceViewModel() : this(null!) { }

    public ObservableCollection<CheckInListItem> CheckIns => _checkIns;

    public ObservableCollection<MemberListItem> Members => _members;

    [ObservableProperty]
    private MemberListItem? _selectedMember;

    [ObservableProperty]
    private string? _statusMessage;

    [ObservableProperty]
    private int _checkedInToday;

    /// <summary>Loads today's log and the member picker.</summary>
    [RelayCommand]
    public async Task LoadAsync()
    {
        ClearError();
        StatusMessage = null;
        IsBusy = true;
        try
        {
            if (dispatcher is null)
            {
                return;
            }

            var members = await dispatcher.Send(new ListMembersQuery(null, null));
            _members.Clear();
            foreach (var member in members)
            {
                // Only members who may actually enter are offered at the desk.
                if (member.Status is "active")
                {
                    _members.Add(member);
                }
            }

            await ReloadLogAsync();
            Title = "Attendance";
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

    [RelayCommand(CanExecute = nameof(HasSelection))]
    private async Task CheckInAsync()
    {
        if (SelectedMember is null)
        {
            return;
        }

        ClearError();
        StatusMessage = null;
        IsBusy = true;
        try
        {
            var result = await dispatcher.Send(new CheckInMemberCommand(SelectedMember.Id));

            if (result.IsFailure)
            {
                // A lapsed membership is an expected refusal, so it is shown as a
                // message rather than logged as a fault.
                ErrorMessage = result.Error.Message;
                return;
            }

            StatusMessage = $"خوش آمدید {result.Value.MemberName} — {result.Value.CheckedInOn}";
            SelectedMember = null;
            await ReloadLogAsync();
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task ReloadLogAsync()
    {
        var log = await dispatcher.Send(new ListCheckInsQuery());
        _checkIns.Clear();
        foreach (var item in log)
        {
            _checkIns.Add(item);
        }

        CheckedInToday = log.Count;
    }

    private bool HasSelection() => SelectedMember is not null;

    partial void OnSelectedMemberChanged(MemberListItem? value) => CheckInCommand.NotifyCanExecuteChanged();
}
