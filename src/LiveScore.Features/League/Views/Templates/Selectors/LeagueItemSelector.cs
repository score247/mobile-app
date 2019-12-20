using LiveScore.Features.League.ViewModels;
using LiveScore.Features.League.ViewModels.LeagueItemViewModels;
using Xamarin.Forms;

namespace LiveScore.Features.League.Views.Templates.Selectors
{
    public class LeagueItemSelector : DataTemplateSelector
    {
        private static readonly DataTemplate LeagueItemTemplate = new LeagueTemplate();
        private static readonly DataTemplate NoFlagLeagueTemplate = new NoFlagLeagueTemplate();

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return item switch
            {
                LeagueViewModel viewModel when viewModel.IsShowFlag => LeagueItemTemplate,
                _ => NoFlagLeagueTemplate,
            };
        }
    }
}