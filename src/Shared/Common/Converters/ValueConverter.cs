using System;
using System.Globalization;
using Xamarin.Forms;

namespace LiveScore.Common.Converters
{
#pragma warning disable S927 // parameter names should match base declaration and other partial definitions

    public abstract class ValueConverter<TSource, TTarget> : IValueConverter
    {
        public abstract TTarget Convert(TSource source, Type targetType, object parameter, CultureInfo culture);

        public virtual TSource ConvertBack(TTarget target, Type targetType, object parameter, CultureInfo culture)
            => default;

        object IValueConverter.Convert(object source, Type targetType, object parameter, CultureInfo culture)

        {
            if (source != null && !(source is TSource))
            {
                throw new InvalidCastException($"The passing type was {source.GetType()} but the expected is {typeof(TSource)}");
            }

            return Convert((TSource)source, targetType, parameter, culture);
        }

        object IValueConverter.ConvertBack(object target, Type targetType, object parameter, CultureInfo culture)
        {
            if (target != null && !(target is TTarget))
            {
                throw new InvalidCastException($"The passing type was {target.GetType()} but the expected is {typeof(TTarget)}");
            }

            return ConvertBack((TTarget)target, targetType, parameter, culture);
        }
    }

#pragma warning restore S927 // parameter names should match base declaration and other partial definitions
}