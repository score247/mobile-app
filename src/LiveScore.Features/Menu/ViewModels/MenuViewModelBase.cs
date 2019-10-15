using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Core;
using LiveScore.Core.ViewModels;
using Prism.Navigation;

namespace LiveScore.Features.Menu.ViewModels
{
    public class MenuViewModelBase : ViewModelBase
    {
        public MenuViewModelBase(INavigationService navigationService, IDependencyResolver serviceLocator)
             : base(navigationService, serviceLocator)
        {
            DoneCommand = new DelegateAsyncCommand(OnDone);
        }

        public DelegateAsyncCommand DoneCommand { get; }

        protected virtual async Task OnDone()
        {
            await NavigationService.GoBackAsync(useModalNavigation: true);
        }
    }
}