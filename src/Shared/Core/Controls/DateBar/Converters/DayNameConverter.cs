using System;
using System.Globalization;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using Xamarin.Forms;

namespace LiveScore.Core.Controls.DateBar.Converters
{
    public class DayNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = System.Convert.ToDateTime(value);

            return (date == DateTime.Today ? AppResources.Today : date.Date.ToDayName()).ToUpperInvariant();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}