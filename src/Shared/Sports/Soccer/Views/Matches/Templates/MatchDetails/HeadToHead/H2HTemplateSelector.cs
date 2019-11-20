using LiveScore.Soccer.ViewModels.MatchDetails.HeadToHead;
using LiveScore.Soccer.Views.Templates.MatchDetails.HeadToHead.CollectionViewTemplates;
using Xamarin.Forms;

namespace LiveScore.Soccer.Views.Templates.MatchDetails.HeadToHead
{
    public class H2HTemplateSelector : DataTemplateSelector
    {
        private static readonly H2HMatchItemTemplate H2HItemTemplate = new H2HMatchItemTemplate();
        private static readonly TeamMatchItemTemplate ResultItemTemplate = new TeamMatchItemTemplate();

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is H2HMatchViewModel itemViewModel)
            {
                if (itemViewModel.IsH2H)
                {
                    return H2HItemTemplate;
                }

                return ResultItemTemplate;
            }

            return H2HItemTemplate;
        }
    }
}
