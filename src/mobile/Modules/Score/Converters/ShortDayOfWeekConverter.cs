namespace Score.Converters
{
    using System;
    using System.Globalization;
    using Xamarin.Forms;

    public class ShortDayOfWeekConverter : IValueConverter
    {
        private const int DayOfWeekLength = 3;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dayOfWeek = value.ToString();

            return dayOfWeek.Substring(0, DayOfWeekLength).ToUpperInvariant();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}