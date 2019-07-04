namespace LiveScore.Soccer.Views.Templates.Odds
{
    using LiveScore.Soccer.ViewModels.DetailOdds.OddItems;
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
