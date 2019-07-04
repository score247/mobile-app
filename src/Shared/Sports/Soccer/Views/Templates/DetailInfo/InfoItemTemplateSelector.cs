namespace LiveScore.Soccer.Views.Templates.MatchDetailInfo
{
    using LiveScore.Soccer.ViewModels.MatchDetailInfo;
    using Xamarin.Forms;

    public class InfoItemTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var itemViewModel = (BaseItemViewModel)item;

            return itemViewModel.CreateTemplate();
        }
    }
}