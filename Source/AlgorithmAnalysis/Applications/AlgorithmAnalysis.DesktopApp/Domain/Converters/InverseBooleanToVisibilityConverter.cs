using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AlgorithmAnalysis.DesktopApp.Domain.Converters
{
    internal sealed class InverseBooleanToVisibilityConverter : IValueConverter
    {
        public InverseBooleanToVisibilityConverter()
        {
        }

        #region IValueConverter Implementation

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = ConverterHelper.ToBoolean(value);

            return !flag ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException($"{nameof(ConvertBack)} is not implemented.");
        }

        #endregion
    }
}
