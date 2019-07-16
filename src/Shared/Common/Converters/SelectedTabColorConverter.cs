using System;
using System.Globalization;
using Xamarin.Forms;

namespace LiveScore.Common.Converters
{
    public class SelectedTabColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (Color)Application.Current.Resources["PrimaryTextColor"];

            if (value != null)
            {
                bool isSelected = (bool)value;

                color = isSelected ? (Color)Application.Current.Resources["ActiveSubTabColor"] : (Color)Application.Current.Resources["PrimaryTextColor"];
            }

            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
