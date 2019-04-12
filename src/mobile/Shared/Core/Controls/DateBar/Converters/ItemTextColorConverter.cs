namespace LiveScore.Core.Controls.DateBar.Converters
{
    using System;
    using System.Globalization;
    using LiveScore.Core.Controls.DateBar.Models;
    using Xamarin.Forms;

    public class ItemTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Color.FromHex("#939393");
            }

            var calendarDate = value as DateBarItem;

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

        private static bool IsToday(DateBarItem calendarDate)
        {
            return calendarDate.Date.Day == DateTime.Today.Day && calendarDate.Date.Month == DateTime.Today.Month && calendarDate.Date.Year == DateTime.Today.Year;
        }
    }
}