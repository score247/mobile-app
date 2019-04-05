namespace Menu.ViewModels
{
    using System.Threading.Tasks;
    using Common.Extensions;
    using Core.Factories;
    using Core.Services;
    using Core.ViewModels;
    using Prism.Navigation;

    public class MenuViewModelBase : ViewModelBase
    {
        public MenuViewModelBase(
            INavigationService navigationService,
            IGlobalFactory globalFactory,
            ISettingsService settingsService) : base(navigationService, globalFactory, settingsService)
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