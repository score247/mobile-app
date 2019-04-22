namespace LiveScore.Menu.ViewModels
{
    using System.Threading.Tasks;
    using Common.Extensions;
    using Core.ViewModels;
    using LiveScore.Core;
    using Prism.Navigation;

    public class MenuViewModelBase : ViewModelBase
    {
        public MenuViewModelBase(INavigationService navigationService, IDepdendencyResolver serviceLocator)
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