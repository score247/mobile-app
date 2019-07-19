namespace LiveScore.Soccer.Views.Templates.DetailOdds
{
    using LiveScore.Soccer.Views.Templates.DetailOdds.OddsItems;
    using Xamarin.Forms;

    public class OddsMovementItemTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return new OneXTwoMovementItemTemplate();
        }
    }
}
