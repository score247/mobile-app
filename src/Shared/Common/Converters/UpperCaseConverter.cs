using System;
using System.Globalization;

namespace LiveScore.Common.Converters
{
    public class UpperCaseConverter : ValueConverter<string, string>
    {
        public override string Convert(string value, Type targetType, object parameter, CultureInfo culture)
            => value?.ToString().ToUpperInvariant();
    }
}