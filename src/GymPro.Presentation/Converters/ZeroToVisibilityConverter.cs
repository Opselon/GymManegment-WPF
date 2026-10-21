using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GymPro.Presentation.Converters;

/// <summary>
/// Shows an element only when a bound count is zero, so a screen can present an
/// empty-state hint without a dedicated property on every view-model.
/// </summary>
public sealed class ZeroToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var count = value switch
        {
            int i => i,
            null => 0,
            _ => 1,
        };

        return count == 0 ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
