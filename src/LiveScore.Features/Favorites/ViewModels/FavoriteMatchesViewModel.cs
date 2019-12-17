using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

namespace LiveScore.Features.Favorites.ViewModels
{
    public class FavoriteMatchesViewModel : TabItemViewModel
    {
        public FavoriteMatchesViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            DataTemplate dataTemplate)
            : base(navigationService, dependencyResolver, dataTemplate, null, AppResources.Matches)
        {
            IsActive = true;
        }
    }
}