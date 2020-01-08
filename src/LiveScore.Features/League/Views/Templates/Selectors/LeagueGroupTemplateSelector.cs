using LiveScore.Features.League.ViewModels.LeagueItemViewModels;
using Xamarin.Forms;

namespace LiveScore.Features.League.Views.Templates.Selectors
{
    public class LeagueGroupTemplateSelector : DataTemplateSelector
    {
        private static readonly DataTemplate LeagueGroupTemplate = new LeagueGroupTemplate();
        private static readonly DataTemplate HasFlagLeaguesGroupTemplate = new HasFlagLeaguesGroupTemplate();

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return item switch
            {
                LeagueGroupViewModel viewModel when viewModel.ShowFlag => HasFlagLeaguesGroupTemplate,
                _ => LeagueGroupTemplate,
            };
        }
    }
}