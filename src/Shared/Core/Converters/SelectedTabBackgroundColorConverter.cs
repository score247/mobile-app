﻿using LiveScore.Common.Converters;
using Xamarin.Forms;

namespace LiveScore.Core.Converters
{
    public class SelectedTabBackgroundColorConverter : ResourceValueConverter<bool?, Color>
    {
        public SelectedTabBackgroundColorConverter() : base(null)
        {
        }

        protected override string GetResourceKey(bool? source)
            => source != null && source.Value
                ? "ActiveSubTabBgColor"
                : "SubTabBgColor";
    }
}