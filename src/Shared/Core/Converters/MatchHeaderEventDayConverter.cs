namespace LiveScore.Core.Converters
{
    using System;
    using System.Globalization;
    using Xamarin.Forms;
    using LiveScore.Common.Extensions;

    public class MatchHeaderEventDayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int eventDay = System.Convert.ToInt32(value.GetType().GetProperty("Day").GetValue(value));
            int eventMonth = System.Convert.ToInt32(value.GetType().GetProperty("Month").GetValue(value));
            int eventYear = System.Convert.ToInt32(value.GetType().GetProperty("Year").GetValue(value));
            var eventDate = new DateTime(eventYear, eventMonth, eventDay);

            return eventDate.ToShortDayMonth();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
