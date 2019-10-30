using System;
using System.Globalization;
using LiveScore.Common.Converters;
using Xamarin.Forms;

namespace LiveScore.Core.Converters
{
    public class SelectedTabColorConverter : ValueConverter<bool?, Color>
    {
        private readonly Func<string, Color> getColor;

        public SelectedTabColorConverter() : this(null)
        {
        }

        public SelectedTabColorConverter(Func<string, Color> getColorHex)
        {
            if (getColorHex == null)
            {
                getColor = (resourceKey) => (Color)Application.Current.Resources[resourceKey];
            }
        }

        public override Color Convert(bool? source, Type targetType, object parameter, CultureInfo culture)
        {
            string resourceKey = source != null && source.Value
             ? "ActiveSubTabColor"
             : "SubTabBgColor";

            return getColor(resourceKey);
        }
    }
}