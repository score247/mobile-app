using System;
using System.Globalization;
using Xamarin.Forms;

namespace LiveScore.Core.Converters
{
    public class MatchMinuteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return value.ToString() + "'";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

