using System;
using System.Globalization;

namespace LiveScore.Common.Converters
{
    public class FirstCharUpperConverter : ValueConverter<string, string>
    {
        public override string Convert(string source, Type targetType, object parameter, CultureInfo culture)
            => source?[0].ToString().ToUpperInvariant();
    }
}
