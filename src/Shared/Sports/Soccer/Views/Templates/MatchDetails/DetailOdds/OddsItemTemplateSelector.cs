namespace LiveScore.Soccer.Views.Templates.MatchDetails.DetailOdds
{
    using LiveScore.Soccer.ViewModels.MatchDetails.DetailOdds.OddItems;
    using Xamarin.Forms;

    public class OddsItemTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var itemViewModel = (BaseItemViewModel)item;

            return itemViewModel.CreateTemplate();
        }
    }
}