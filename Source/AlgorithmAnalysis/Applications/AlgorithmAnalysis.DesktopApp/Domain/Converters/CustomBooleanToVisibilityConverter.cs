using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AlgorithmAnalysis.DesktopApp.Domain.Converters
{
    internal sealed class CustomBooleanToVisibilityConverter : IValueConverter
    {
        public CustomBooleanToVisibilityConverter()
        {
        }

        #region IValueConverter Implementation

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = false;
            if (value is bool valueConverted)
            {
                flag = valueConverted;
            }
            else if (value is bool?)
            {
                var nullable = (bool?) value;
                flag = nullable.GetValueOrDefault(false);
            }

            return flag ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException($"{nameof(ConvertBack)} is not implemented.");
        }

        #endregion
    }
}
