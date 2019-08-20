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
                return (Color)Application.Current.Resources["FourthTextColor"];
            }

            var calendarDate = value as DateBarItem;

            if (calendarDate.IsSelected)
            {
                return (Color)Application.Current.Resources["PrimaryAccentColor"];
            }

            if (IsToday(calendarDate))
            {
                return Color.White;
            }

            return (Color)Application.Current.Resources["FourthTextColor"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        private static bool IsToday(DateBarItem dateBarItem) 
            => dateBarItem.Date.Date == DateTime.Now.Date;
    }
}