using System.Globalization;
using System.Windows.Data;

namespace GymPro.Presentation.Converters;

/// <summary>
/// Inverts a boolean. Used to disable buttons while a command is running, which
/// XAML cannot express directly.
/// </summary>
[ValueConversion(typeof(bool), typeof(bool))]
public sealed class InverseBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is bool b && !b;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => value is bool b && !b;
}
