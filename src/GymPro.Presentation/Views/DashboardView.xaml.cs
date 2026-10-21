using System.Windows.Controls;
using GymPro.Presentation.ViewModels;

namespace GymPro.Presentation.Views;

public partial class DashboardView : UserControl
{
    public DashboardView(DashboardViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
