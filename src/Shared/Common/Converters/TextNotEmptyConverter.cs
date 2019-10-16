using System;
using System.Globalization;

namespace LiveScore.Common.Converters
{
    public class TextNotEmptyConverter : ValueConverter<string, bool>
    {
        public override bool Convert(string value, Type targetType, object parameter, CultureInfo culture)
            => !string.IsNullOrWhiteSpace(value);
    }
}