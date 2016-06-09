using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Elusive.Solodex.UI.Common.ValueConverters
{
    /// <summary>
    ///     Value converter for making a boolean value a visibility value
    /// </summary>
    [ValueConversion(typeof (bool), typeof (Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        ///     Converts a boolean value to a visibility value.
        ///     True = Visible
        ///     False = Collapsed
        /// </summary>
        /// <param name="value">boolean value</param>
        /// <param name="targetType">the type of the value param</param>
        /// <param name="parameter">Pass in "invert" to flip converted value</param>
        /// <param name="culture">current culture</param>
        /// <returns>Visibility</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = Visibility.Collapsed;
            var b = (bool) value;
            var str = parameter as string;
            if (!string.IsNullOrWhiteSpace(str))
            {
                if (str.Contains("invert"))
                {
                    b = !b;
                }
                if (str.Contains("hide") || str.Contains("hidden"))
                {
                    result = Visibility.Hidden;
                }
            }

            if (b)
            {
                result = Visibility.Visible;
            }
            return result;
        }

        /// <summary>
        ///     Not Implemented
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}