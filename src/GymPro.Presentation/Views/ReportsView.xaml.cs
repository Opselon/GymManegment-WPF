using System.Windows.Controls;
using GymPro.Presentation.ViewModels;

namespace GymPro.Presentation.Views;

public partial class ReportsView : UserControl
{
    public ReportsView(ReportsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
