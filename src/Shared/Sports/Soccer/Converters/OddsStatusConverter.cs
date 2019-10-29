using System;
using System.Globalization;
using LiveScore.Common.Converters;
using LiveScore.Core.Enumerations;
using Xamarin.Forms;

namespace LiveScore.Soccer.Converters
{
    public class OddsStatusConverter : ValueConverter<string, Color>
    {
        public override Color Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color;

            if (string.IsNullOrWhiteSpace(value))
            {
                return (Color)Application.Current.Resources["PrimaryTextColor"];
            }

            if (value.Equals(OddsTrend.Up.Value.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                color = (Color)Application.Current.Resources["UpLiveOddColor"];
            }
            else if (value.Equals(OddsTrend.Down.Value.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                color = (Color)Application.Current.Resources["DownLiveOddColor"];
            }
            else
            {
                color = (Color)Application.Current.Resources["PrimaryTextColor"];
            }


            return color;
        }
    }
}