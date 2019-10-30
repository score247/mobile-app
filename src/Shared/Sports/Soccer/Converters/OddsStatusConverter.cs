using System;
using LiveScore.Common.Converters;
using LiveScore.Core.Enumerations;
using Xamarin.Forms;

namespace LiveScore.Soccer.Converters
{
    public class OddsStatusConverter : ResourceValueConverter<string, Color>
    {
        public OddsStatusConverter() : base(null)
        {
        }

        protected override string GetResourceKey(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return "PrimaryTextColor";
            }

            if (source.Equals(OddsTrend.Up.Value.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return "UpLiveOddColor";
            }
            else if (source.Equals(OddsTrend.Down.Value.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return "DownLiveOddColor";
            }
            else
            {
                return "PrimaryTextColor";
            }
        }
    }
}