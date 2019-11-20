using LiveScore.Soccer.ViewModels.Leagues.LeagueDetails.Table;
using LiveScore.Soccer.Views.Leagues.Templates.LeagueDetails.Fixtures;
using LiveScore.Soccer.Views.Leagues.Templates.LeagueDetails.Table;
using Xamarin.Forms;

namespace LiveScore.Soccer.Views.Leagues.Templates
{
    public class LeagueDetailItemTemplateSelector : DataTemplateSelector
    {
        private static readonly DataTemplate TableTemplate = new TableTemplate();
        private static readonly DataTemplate FixturesTemplate = new FixturesTemplate();

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is TableViewModel)
            {
                return TableTemplate;
            }

            return FixturesTemplate;
        }
    }
}