namespace LiveScore.Soccer.Views.Templates.DetailOdds
{
    using LiveScore.Soccer.ViewModels.DetailOdds.OddItems;
    using Xamarin.Forms;

    public class OddsMovementItemTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var itemViewModel = (BaseMovementItemViewModel)item;

            return itemViewModel.CreateTemplate();
        }
    }
}
