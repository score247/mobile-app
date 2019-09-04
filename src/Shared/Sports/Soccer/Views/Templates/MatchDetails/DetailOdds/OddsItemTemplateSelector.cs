namespace LiveScore.Soccer.Views.Templates.MatchDetails.DetailOdds
{
    using Xamarin.Forms;
    using LiveScore.Soccer.ViewModels.MatchDetails.DetailOdds.OddItems;

    public class OddsItemTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var itemViewModel = (BaseItemViewModel)item;

            return itemViewModel.CreateTemplate();
        }
    }
}