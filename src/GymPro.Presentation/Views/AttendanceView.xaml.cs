using System.Windows.Controls;
using GymPro.Presentation.ViewModels;

namespace GymPro.Presentation.Views;

public partial class AttendanceView : UserControl
{
    public AttendanceView(AttendanceViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
