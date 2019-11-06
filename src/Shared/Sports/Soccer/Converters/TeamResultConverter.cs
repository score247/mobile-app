using System;
using LiveScore.Common.Converters;
using LiveScore.Core.Enumerations;
using Xamarin.Forms;

namespace LiveScore.Soccer.Converters
{
    public class TeamResultConverter : ResourceValueConverter<string, Color>
    {
        public TeamResultConverter()
            : base(null)
        {
        }

        protected TeamResultConverter(Func<string, Color> GetResourceValue)
            : base(GetResourceValue)
        {
        }

        protected override string GetResourceKey(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return "DrawIconColor";
            }

            if (source.Equals(TeamResult.Win.DisplayName, StringComparison.OrdinalIgnoreCase))
            {
                return "WinIconColor";
            }

            if (source.Equals(TeamResult.Draw.DisplayName, StringComparison.OrdinalIgnoreCase))
            {
                return "DrawIconColor";
            }

            return "LoseIconColor";
        }
    }
}
