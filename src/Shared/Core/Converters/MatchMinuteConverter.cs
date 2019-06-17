﻿namespace LiveScore.Core.Converters
{
    using System;
    using System.Globalization;
    using Xamarin.Forms;

    public class MatchMinuteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return $"{value}'";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}