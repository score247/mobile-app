namespace Score.Converters
{
    using System;
    using System.Globalization;
    using Common.Contants;
    using Common.LangResources;
    using Xamarin.Forms;

    public class MatchStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString() == SportRadarStatus.Ended ? AppResources.FullTime : value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}