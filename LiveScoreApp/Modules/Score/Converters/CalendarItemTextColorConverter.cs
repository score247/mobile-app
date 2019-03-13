namespace Score.Converters
{
    using System;
    using System.Globalization;
    using Xamarin.Forms;

    public class CalendarItemTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isSelected = System.Convert.ToBoolean(value);

            return isSelected ? Color.FromHex("#F24822") : Color.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}