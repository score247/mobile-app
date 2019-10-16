﻿namespace LiveScore.Core.Converters
{
    using System;
    using System.Globalization;
    using Xamarin.Forms;

    public class FirstCharConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value == null || value.ToString()?.Length == 0
                ? string.Empty
                : value.ToString().Substring(0, 1).ToUpperInvariant();

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString();
        }
    }
}