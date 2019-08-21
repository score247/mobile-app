namespace LiveScore.Core.Converters
{
    using System;
    using System.Globalization;
    using LiveScore.Common.Extensions;
    using Xamarin.Forms;

    public class MatchHeaderEventDayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueType = value.GetType();

            int eventDay = System.Convert.ToInt32(valueType.GetProperty("Day").GetValue(value));
            int eventMonth = System.Convert.ToInt32(valueType.GetProperty("Month").GetValue(value));
            int eventYear = System.Convert.ToInt32(valueType.GetProperty("Year").GetValue(value));
            // var eventDate = new DateTimeOffset(new DateTime(eventYear, eventMonth, eventDay));

            return DateTimeOffset.Now.ToLocalShortDayMonth();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}