using System;
using System.Globalization;
using Xamarin.Forms;

namespace LiveScore.Core.Controls.DateBar.Converters
{
    public class ActiveColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isActive = System.Convert.ToBoolean(value);

            return isActive
                ? (Color)Application.Current.Resources["DateBarSelectedTextColor"]
                : (Color)Application.Current.Resources["DateBarTextColor"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}