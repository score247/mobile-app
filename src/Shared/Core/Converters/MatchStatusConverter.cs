namespace LiveScore.Core.Converters
{
    using System;
    using System.Globalization;
    using LiveScore.Common.LangResources;
    using LiveScore.Core.Enumerations;
    using Xamarin.Forms;

    public class MatchStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (MatchStatus)value;
            var convertStatus = string.Empty;

            if (status != null)
            {
                convertStatus = status.IsClosed ? AppResources.FullTime : value.ToString();
            }

            return convertStatus;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}