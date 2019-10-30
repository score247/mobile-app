using System;
using System.Globalization;
using LiveScore.Common.Converters;
using Xamarin.Forms;

namespace LiveScore.Core.Converters
{
    public class SelectedTabBackgroundColorConverter : ValueConverter<bool?, Color>
    {
        private readonly Func<string, Color> getColor;

        public SelectedTabBackgroundColorConverter() : this(null)
        {
        }

        public SelectedTabBackgroundColorConverter(Func<string, Color> getColorHex)
        {
            if (getColorHex == null)
            {
                getColor = (resourceKey) => (Color)Application.Current.Resources[resourceKey];
            }
        }

        public override Color Convert(bool? source, Type targetType, object parameter, CultureInfo culture)
        {
            string resourceKey = source != null && source.Value
                ? "ActiveSubTabBgColor"
                : "SubTabBgColor";

            return getColor(resourceKey);
        }
    }
}