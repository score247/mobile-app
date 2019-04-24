namespace LiveScore.Core.Controls.DateBar.Converters
{
    using System;
    using System.Globalization;
    using Xamarin.Forms;

    public class HomeTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isSelectHome = (bool)value;
            var selectedColor = (Color) Application.Current.Resources["PrimaryAccentColor"];
            var unselectedColor = (Color)Application.Current.Resources["FourthTextColor"];
            
            return isSelectHome ? selectedColor : unselectedColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}