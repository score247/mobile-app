namespace LiveScore.Soccer.Converters
{

    using System;
    using System.Globalization;
    using LiveScore.Core.Enumerations;
    using Xamarin.Forms;

    public class OddsStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (Color)Application.Current.Resources["PrimaryTextColor"];

            if (value != null)
            {
                string valueAsString = value.ToString();

                if (valueAsString.Equals(OddsTrend.Up.Value, StringComparison.OrdinalIgnoreCase))
                {
                    color = (Color)Application.Current.Resources["UpLiveOddColor"];
                }
                else if (valueAsString.Equals(OddsTrend.Down.Value, StringComparison.OrdinalIgnoreCase))
                {
                    color = (Color)Application.Current.Resources["DownLiveOddColor"];
                }
                else
                {
                    color = (Color)Application.Current.Resources["PrimaryTextColor"];
                }
            }

            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
