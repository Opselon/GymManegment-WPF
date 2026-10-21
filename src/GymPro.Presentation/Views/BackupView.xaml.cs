using System.Windows.Controls;
using GymPro.Presentation.ViewModels;

namespace GymPro.Presentation.Views;

public partial class BackupView : UserControl
{
    public BackupView(BackupViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
