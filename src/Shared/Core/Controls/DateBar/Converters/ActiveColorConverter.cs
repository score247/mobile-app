using LiveScore.Common.Converters;
using Xamarin.Forms;

namespace LiveScore.Core.Controls.DateBar.Converters
{
    public class ActiveColorConverter : ResourceValueConverter<bool, Color>
    {
        protected override string GetResourceKey(bool source) 
            => source
                 ? "DateBarSelectedTextColor"
                 : "DateBarTextColor";
    }
}