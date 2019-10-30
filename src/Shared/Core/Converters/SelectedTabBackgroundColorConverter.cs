using System;
using LiveScore.Common.Converters;
using Xamarin.Forms;

namespace LiveScore.Core.Converters
{
    public class SelectedTabBackgroundColorConverter : ResourceValueConverter<bool?, Color>
    {
        public SelectedTabBackgroundColorConverter() 
            : base(null)
        {
        }

        protected SelectedTabBackgroundColorConverter(Func<string, Color> GetResourceValue) 
            : base(GetResourceValue) { }

        protected override string GetResourceKey(bool? source)
            => source != null && source.Value
                ? "ActiveSubTabBgColor"
                : "SubTabBgColor";
    }
}