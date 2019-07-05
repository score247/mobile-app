namespace LiveScore.Soccer.Converters
{

    using System;
    using System.Globalization;
    using Xamarin.Forms;

    public class OddsStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = Color.White;

            if (value != null)
            {
                string valueAsString = value.ToString();
                switch (valueAsString)
                {
                    case ("up"):
                        {
                            color = Color.Red;
                            break;
                        }
                    case ("down"):
                        {
                            color = Color.Green;
                            break;
                        }
                    default:
                        {
                            color = Color.White;
                            break;
                        }
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
