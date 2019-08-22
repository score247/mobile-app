namespace LiveScore.Soccer.Extensions
{
    using System.Globalization;

    public static class BetOptionOddsExtension
    {
        private const string OddsNumerFormat = "0.00";

        public static string ToOddsFormat(this decimal value)
            => value.ToString(OddsNumerFormat);

        public static string ToOddsOptionFormat(this string value)
        {
            var isParsed = float.TryParse(value, out float newValue);

            return isParsed
                ? newValue.ToString(CultureInfo.InvariantCulture)
                : value;
        }
    }
}