using LiveScore.Soccer.ViewModels.MatchDetails.HeadToHead;
using LiveScore.Soccer.Views.Templates.MatchDetails.HeadToHead.CollectionViewTemplates;
using Xamarin.Forms;

namespace LiveScore.Soccer.Views.Templates.MatchDetails.HeadToHead
{
    public class H2HTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is SummaryMatchViewModel)
            {
                var itemViewModel = item as SummaryMatchViewModel;

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
