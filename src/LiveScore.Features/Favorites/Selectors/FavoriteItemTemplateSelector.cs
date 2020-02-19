using LiveScore.Features.Favorites.ViewModels;
using LiveScore.Features.Favorites.Views;
using Xamarin.Forms;

namespace LiveScore.Features.Favorites.Selectors
{
    public class FavoriteItemTemplateSelector : DataTemplateSelector
    {
        private static readonly DataTemplate FavoriteMatches = new FavoriteMatchesTemplate();
        private static readonly DataTemplate FavoriteLeagues = new FavoriteLeaguesTemplate();
        private static readonly DataTemplate FavoriteTeams = new FavoriteTeamsTemplate();

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return item switch
            {
                FavoriteMatchesViewModel _ => FavoriteMatches,
                FavoriteLeaguesViewModel _ => FavoriteLeagues,
                FavoriteTeamsViewModel _ => FavoriteTeams,

                _ => FavoriteMatches,
            };
        }
    }
}