using LiveScore.Soccer.ViewModels.MatchDetails.HeadToHead;
using LiveScore.Soccer.Views.Templates.MatchDetails.HeadToHead.CollectionViewTemplates;
using Xamarin.Forms;

namespace LiveScore.Soccer.Views.Templates.MatchDetails.HeadToHead
{
    public class H2HTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is H2HMatchViewModel)
            {
                var itemViewModel = item as H2HMatchViewModel;

                if (itemViewModel.IsH2H)
                {
                    return new H2HMatchItemTemplate();
                }

                return new TeamMatchItemTemplate();
            }

            return new H2HMatchItemTemplate();
        }
    }
}
