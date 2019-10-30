using System;
using System.Globalization;

namespace LiveScore.Common.Converters
{
    public class UpperCaseConverter : ValueConverter<string, string>
    {
        public override string Convert(string source, Type targetType, object parameter, CultureInfo culture)
            => source?.ToString().ToUpperInvariant();
    }
}