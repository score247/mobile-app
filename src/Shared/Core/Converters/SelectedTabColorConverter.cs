using LiveScore.Common.Converters;
using Xamarin.Forms;

namespace LiveScore.Core.Converters
{
    public class SelectedTabColorConverter : ResourceValueConverter<bool?, Color>
    {
        public SelectedTabColorConverter() : base(null)
        {
        }

        protected override string GetResourceKey(bool? source)
            => source != null && source.Value
             ? "ActiveSubTabColor"
             : "SubTabColor";
    }
}