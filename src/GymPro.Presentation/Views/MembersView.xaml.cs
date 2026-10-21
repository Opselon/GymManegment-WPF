using System.Windows.Controls;
using GymPro.Presentation.ViewModels;

namespace GymPro.Presentation.Views;

public partial class MembersView : UserControl
{
    public MembersView(MembersViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
