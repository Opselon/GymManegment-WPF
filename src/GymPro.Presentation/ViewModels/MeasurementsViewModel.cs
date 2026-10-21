using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymPro.Application.Abstractions;
using GymPro.Application.Attendance.Commands.RecordMeasurement;
using GymPro.Application.Attendance.Queries.ListMeasurements;
using GymPro.Application.Memberships.Queries.ListMembers;
using GymPro.Domain.ValueObjects;

namespace GymPro.Presentation.ViewModels;

/// <summary>
/// Measurement-history screen. A member is picked first; the whole screen is
/// inert until then, because a measurement without a member has no meaning.
/// </summary>
public sealed partial class MeasurementsViewModel : ViewModelBase
{
    private readonly IRequestDispatcher? _dispatcher;
    private readonly TimeProvider _clock;
    private readonly ObservableCollection<MemberListItem> _members = [];
    private readonly ObservableCollection<MeasurementListItem> _measurements = [];

    public MeasurementsViewModel(IRequestDispatcher dispatcher, TimeProvider clock)
    {
        _dispatcher = dispatcher;
        _clock = clock;

        var today = JalaliDate.Today(clock);
        _year = today.Year;
        _month = today.Month;
        _day = today.Day;
    }

    public MeasurementsViewModel() : this(null!, TimeProvider.System) { }

    public ObservableCollection<MemberListItem> Members => _members;

    public ObservableCollection<MeasurementListItem> Measurements => _measurements;

    [ObservableProperty]
    private MemberListItem? _selectedMember;

    [ObservableProperty]
    private int _year;

    [ObservableProperty]
    private int _month;

    [ObservableProperty]
    private int _day;

    [ObservableProperty]
    private string? _chestCm;

    [ObservableProperty]
    private string? _armCm;

    [ObservableProperty]
    private string? _waistCm;

    [ObservableProperty]
    private string? _heightCm;

    [ObservableProperty]
    private string? _weightKg;

    [ObservableProperty]
    private bool _hasHistory;

    [ObservableProperty]
    private bool _canSave;

    /// <summary>Confirmation line; informational, so it is not an error.</summary>
    [ObservableProperty]
    private string? _statusMessage;

    [RelayCommand]
    public async Task LoadAsync()
    {
        ClearError();

        if (_dispatcher is null)
        {
            return;
        }

        IsBusy = true;
        try
        {
            var members = await _dispatcher.Send(new ListMembersQuery(null, null));
            _members.Clear();
            foreach (var member in members)
            {
                _members.Add(member);
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    partial void OnSelectedMemberChanged(MemberListItem? value)
    {
        ReevaluateCanSave();
        _ = LoadHistoryCoreAsync();
    }

    partial void OnChestCmChanged(string? value) => ReevaluateCanSave();

    partial void OnArmCmChanged(string? value) => ReevaluateCanSave();

    partial void OnWaistCmChanged(string? value) => ReevaluateCanSave();

    partial void OnHeightCmChanged(string? value) => ReevaluateCanSave();

    partial void OnWeightKgChanged(string? value) => ReevaluateCanSave();

    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task SaveAsync()
    {
        ClearError();
        StatusMessage = null;

        if (_dispatcher is null || SelectedMember is null)
        {
            return;
        }

        IsBusy = true;
        try
        {
            var result = await _dispatcher.Send(new RecordMeasurementCommand(
                SelectedMember.Id,
                Year, Month, Day,
                ChestCm: ParseCm(ChestCm),
                ArmCm: ParseCm(ArmCm),
                WaistCm: ParseCm(WaistCm),
                HeightCm: ParseCm(HeightCm),
                WeightKg: ParseCm(WeightKg)));

            if (result.IsFailure)
            {
                ErrorMessage = result.Error.Message;
                return;
            }

            ChestCm = null;
            ArmCm = null;
            WaistCm = null;
            HeightCm = null;
            WeightKg = null;
            StatusMessage = "اندازه‌گیری ثبت شد.";

            await LoadHistoryCoreAsync();
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>Empty boxes stay empty; a number is a number.</summary>
    private static int? ParseCm(string? text)
        => int.TryParse(text, out var value) ? value : null;

    /// <summary>
    /// A save needs a member and at least one value. Each field re-runs this so
    /// the button enables as soon as the operator types the first number.
    /// </summary>
    private void ReevaluateCanSave()
    {
        CanSave = SelectedMember is not null
            && (ParseCm(ChestCm) is not null
                || ParseCm(ArmCm) is not null
                || ParseCm(WaistCm) is not null
                || ParseCm(HeightCm) is not null
                || ParseCm(WeightKg) is not null);

        SaveCommand.NotifyCanExecuteChanged();
    }

    private async Task LoadHistoryCoreAsync()
    {
        ClearError();

        if (_dispatcher is null)
        {
            return;
        }

        if (SelectedMember is null)
        {
            _measurements.Clear();
            HasHistory = false;
            return;
        }

        IsBusy = true;
        try
        {
            var history = await _dispatcher.Send(new ListMeasurementsQuery(SelectedMember.Id));
            _measurements.Clear();
            foreach (var row in history)
            {
                _measurements.Add(row);
            }

            HasHistory = _measurements.Count > 0;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
