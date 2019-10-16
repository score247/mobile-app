using System;
using System.Globalization;
using Xamarin.Forms;

namespace LiveScore.Common.Converters
{
    public abstract class ValueConverter<TSource, TTarget> : IValueConverter
    {
        public abstract TTarget Convert(TSource value, Type targetType, object parameter, CultureInfo culture);

        public virtual TSource ConvertBack(TTarget value, Type targetType, object parameter, CultureInfo culture)
            => default;

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && !(value is TSource))
            {
                throw new InvalidCastException($"The passing type was {value.GetType()} but the expected is {typeof(TSource)}");
            }

            return Convert((TSource)value, targetType, parameter, culture);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && !(value is TTarget))
            {
                throw new InvalidCastException($"The passing type was {value.GetType()} but the expected is {typeof(TTarget)}");
            }

            return ConvertBack((TTarget)value, targetType, parameter, culture);
        }
    }
}