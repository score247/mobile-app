namespace LiveScore.Core.Converters
{
    using System;
    using System.Globalization;
    using Xamarin.Forms;

    public class SelectedTabColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (Color)Application.Current.Resources["SubTabColor"];

            if (value != null)
            {
                bool isSelected = (bool)value;

                color = isSelected ? (Color)Application.Current.Resources["ActiveSubTabColor"] : (Color)Application.Current.Resources["SubTabColor"];
            }

            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}