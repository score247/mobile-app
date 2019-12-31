using LiveScore.Soccer.ViewModels.Matches.MatchDetails.Statistics;
using Xamarin.Forms;

namespace LiveScore.Soccer.Views.Templates.MatchDetails.Statistics
{
    public class StatisticItemTemplateSelector : DataTemplateSelector
    {
        private static readonly DataTemplate possessionTemplate = new PossessionTemplate();
        private static readonly DataTemplate statisticItemTemplate = new StatisticsItemTemplate();

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var itemViewModel = (StatisticsItemViewModel)item;

            return itemViewModel != null && itemViewModel.IsPossessionStatistic
                ? possessionTemplate
                : statisticItemTemplate;
        }
    }
}