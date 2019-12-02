using Xamarin.Forms;

namespace LiveScore.Common.Converters
{
    public class ColorConverter : ResourceValueConverter<string, Color>
    {
        protected override string GetResourceKey(string source) => source;
    }
}