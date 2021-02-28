using System;
using System.Globalization;
using System.Windows.Data;

namespace Umator.Plugins.WinServices.UI.Converters
{ 
    public class InvertBooleanConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && Boolean.TryParse(value.ToString(), out var boolValue))
            {
                return !boolValue;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && Boolean.TryParse(value.ToString(), out var boolValue))
            {
                return !boolValue;
            }

            return false;
        }
    }
}
