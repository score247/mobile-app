namespace LiveScore.Soccer.Views.Templates.MatchDetailInfo
{
    using LiveScore.Soccer.ViewModels.MatchDetailInfo;
    using Xamarin.Forms;

    public class MatchDetailInfoItemTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var itemViewModel = (BaseInfoItemViewModel)item;

            return itemViewModel.CreateTemplate();
        }
    }
}