using System;
using System.Globalization;
using Xamarin.Forms;

namespace LiveScore.Common.Converters
{
    public abstract class ResourceValueConverter<TSource, TTarget> : ValueConverter<TSource, TTarget>
    {
        protected readonly Func<string, TTarget> GetResourceValue;

        protected ResourceValueConverter(Func<string, TTarget> GetResource = null)
        {
            if (GetResource == null)
            {
                GetResourceValue = (resourceKey) => (TTarget)Application.Current.Resources[resourceKey];
            }
        }

        public override TTarget Convert(TSource source, Type targetType, object parameter, CultureInfo culture)
            => GetResourceValue(GetResourceKey(source));

        protected abstract string GetResourceKey(TSource source);
    }
}