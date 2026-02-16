using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Presentation.Wpf.Services
{
    /// <summary>
    /// Converts a string to Visibility:
    /// - null/empty/whitespace => Collapsed
    /// - otherwise => Visible
    ///
    /// Used for showing/hiding an error banner based on ErrorMessage.
    /// </summary>
    public sealed class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value as string;
            return string.IsNullOrWhiteSpace(s) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
