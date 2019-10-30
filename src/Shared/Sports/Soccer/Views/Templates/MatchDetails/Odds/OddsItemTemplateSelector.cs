using LiveScore.Soccer.ViewModels.MatchDetails.Odds.OddItems;
using LiveScore.Soccer.Views.Templates.MatchDetails.Odds.OddsItems.AsianHdp;
using Xamarin.Forms;

namespace LiveScore.Soccer.Views.Templates.MatchDetails.Odds
{
    public class OddsItemTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        => (item is BaseItemViewModel itemViewModel)
                        ? itemViewModel.CreateTemplate()
                        : new AsianHdpItemTemplate();
    }
}