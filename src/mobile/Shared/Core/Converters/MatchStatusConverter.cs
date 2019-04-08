namespace LiveScore.Core.Converters
{
    using System;
    using System.Globalization;
    using LiveScore.Common.LangResources;
    using LiveScore.Core.Constants;
    using Xamarin.Forms;

    public class MatchStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value == null || value.ToString() == SportRadarStatus.Ended) ? AppResources.FullTime : value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}