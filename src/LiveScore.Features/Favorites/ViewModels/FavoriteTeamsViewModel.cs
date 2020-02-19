using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

namespace LiveScore.Features.Favorites.ViewModels
{
    public class FavoriteTeamsViewModel : TabItemViewModel
    {
        public FavoriteTeamsViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(navigationService, dependencyResolver, null, eventAggregator, AppResources.Teams)
        {
        }
    }
}