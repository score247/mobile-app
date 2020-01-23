using System;
using System.Globalization;
using LiveScore.Common.Extensions;
using Xamarin.Forms;

namespace LiveScore.Core.Controls.DateBar.Converters
{
    public class DayMonthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = System.Convert.ToDateTime(value);

            return date.ToDayMonth().ToUpperInvariant();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}