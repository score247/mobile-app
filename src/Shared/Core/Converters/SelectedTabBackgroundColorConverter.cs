namespace LiveScore.Core.Converters
{
    using System;
    using System.Globalization;
    using Xamarin.Forms;

    public class SelectedTabBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (Color)Application.Current.Resources["SubTabBgColor"];

            if (value != null)
            {
                bool isSelected = (bool)value;

                color = isSelected ? (Color)Application.Current.Resources["ActiveSubTabBgColor"] : (Color)Application.Current.Resources["SubTabBgColor"];
            }

            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
