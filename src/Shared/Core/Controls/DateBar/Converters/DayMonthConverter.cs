using System;
using System.Globalization;
using Xamarin.Forms;

namespace LiveScore.Core.Controls.DateBar.Converters
{
    public class DayMonthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = System.Convert.ToDateTime(value);

            return date.ToString("dd MMM").ToUpperInvariant();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}