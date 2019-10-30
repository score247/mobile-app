using System;
using System.Globalization;

namespace LiveScore.Common.Converters
{
    public class TextNotEmptyConverter : ValueConverter<string, bool>
    {
        public override bool Convert(string source, Type targetType, object parameter, CultureInfo culture)
            => !string.IsNullOrWhiteSpace(source);
    }
}