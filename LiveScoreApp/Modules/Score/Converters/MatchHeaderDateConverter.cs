namespace Score.Converters
{
    using System;
    using System.Globalization;
    using Common.Models.MatchInfo;
    using Xamarin.Forms;

    public class MatchHeaderDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var eventDay = System.Convert.ToInt32(value);
            var eventDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, eventDay);

            return eventDate.ToString("dd MMM");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}