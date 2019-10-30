﻿using System;
using System.Globalization;
using Xamarin.Forms;

namespace LiveScore.Common.Converters
{
    public abstract class ResourceValueConverter<TSource, TTarget> : ValueConverter<TSource, TTarget>
    {
        protected readonly Func<string, TTarget> GetResource;

        protected ResourceValueConverter(Func<string, TTarget> GetResource = null)
        {
            if (GetResource == null)
            {
                this.GetResource = (resourceKey) => (TTarget)Application.Current.Resources[resourceKey];
            }
        }

        public override TTarget Convert(TSource source, Type targetType, object parameter, CultureInfo culture)
            => GetResource(GetResourceKey(source));

        protected abstract string GetResourceKey(TSource source);
    }
}