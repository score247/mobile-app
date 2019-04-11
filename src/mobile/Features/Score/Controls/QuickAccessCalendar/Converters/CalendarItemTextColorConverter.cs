namespace LiveScore.Score.Controls.QuickAccessCalendar.Converters
{
    using System;
    using System.Globalization;
    using LiveScore.Score.Controls.QuickAccessCalendar.Models;
    using Xamarin.Forms;

    public class CalendarItemTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Color.FromHex("#939393");
            }

            var calendarDate = value as QuickAccessCalendarDate;

            if (calendarDate.IsSelected)
            {
                return Color.FromHex("#F24822");
            }

            if (IsToday(calendarDate))
            {
                return Color.White;
            }

            return Color.FromHex("#939393");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        private static bool IsToday(QuickAccessCalendarDate calendarDate)
        {
            return calendarDate.Date.Day == DateTime.Today.Day && calendarDate.Date.Month == DateTime.Today.Month && calendarDate.Date.Year == DateTime.Today.Year;
        }
    }
}