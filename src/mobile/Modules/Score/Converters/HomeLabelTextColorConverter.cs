namespace Score.Converters
{
    using System;
    using System.Globalization;
    using Xamarin.Forms;

    public class HomeLabelTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isSelectHome = (bool)value;

            return isSelectHome ? Color.FromHex("#F24822") : Color.FromHex("#939393");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}