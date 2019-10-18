namespace LiveScore.Soccer.Views.Templates.MatchDetails.DetailOdds
{
    using LiveScore.Soccer.ViewModels.MatchDetails.DetailOdds.OddItems;
    using LiveScore.Soccer.Views.Templates.MatchDetails.DetailOdds.OddsItems.AsianHdp;
    using Xamarin.Forms;

    public class OddsMovementItemTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
            => (item is BaseMovementItemViewModel itemViewModel)
                        ? itemViewModel.CreateTemplate()
                        : new AsianHdpMovementItemTemplate();
    }
}