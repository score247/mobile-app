namespace LiveScore.Menu.ViewModels
{
    using System.Threading.Tasks;
    using Common.Extensions;
    using Core.ViewModels;
    using LiveScore.Core.Factories;
    using Prism.Navigation;

    public class MenuViewModelBase : ViewModelBase
    {
        public MenuViewModelBase(INavigationService navigationService, IServiceLocator serviceLocator)
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