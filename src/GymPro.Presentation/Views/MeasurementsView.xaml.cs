using System.Windows.Controls;
using GymPro.Presentation.ViewModels;

namespace GymPro.Presentation.Views;

public partial class MeasurementsView : UserControl
{
    public MeasurementsView(MeasurementsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
