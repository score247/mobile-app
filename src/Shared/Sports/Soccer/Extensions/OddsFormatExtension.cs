namespace LiveScore.Soccer.Extensions
{
    using System.Globalization;

    public static class BetOptionOddsExtension
    {
        private const string OddsNumberFormat = "0.00";

        public static string ToOddsFormat(this decimal value)
            => value.ToString(OddsNumberFormat);

        public static string ToOddsOptionFormat(this string value)
        {
            var isParsed = float.TryParse(value, out var newValue);

            return isParsed
                ? newValue.ToString(CultureInfo.InvariantCulture)
                : value;
        }
    }
}