using System;
using LiveScore.Common.Converters;
using Xamarin.Forms;

namespace LiveScore.Core.Converters
{
    public class SelectedTabColorConverter : ResourceValueConverter<bool?, Color>
    {
        public SelectedTabColorConverter()
            : base(null)
        {
        }

        protected SelectedTabColorConverter(Func<string, Color> GetResource)
            : base(GetResource) { }

        protected override string GetResourceKey(bool? source)
            => source != null && source.Value
             ? "ActiveSubTabColor"
             : "SubTabColor";
    }
}