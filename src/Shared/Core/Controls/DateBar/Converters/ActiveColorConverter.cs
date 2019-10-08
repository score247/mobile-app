using System;
using System.Globalization;
using Xamarin.Forms;

namespace LiveScore.Core.Controls.DateBar.Converters
{
    public class ActiveColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isActive = System.Convert.ToBoolean(value);

            return isActive
                ? Color.FromHex("#FF7246")
                : Color.FromHex("#969697");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}