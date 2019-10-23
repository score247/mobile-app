using System;
using System.Globalization;
using Xamarin.Forms;

namespace LiveScore.Core.Converters
{
    public class SelectedTabBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (Color)Application.Current.Resources["SubTabBgColor"];

            if (value == null)
            {
                return color;
            }

            var isSelected = (bool)value;

            return isSelected
                ? (Color)Application.Current.Resources["ActiveSubTabBgColor"]
                : (Color)Application.Current.Resources["SubTabBgColor"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString();
        }
    }
}