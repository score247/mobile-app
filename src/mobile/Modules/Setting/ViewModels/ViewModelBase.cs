namespace Setting.ViewModels
{
    using System.Threading.Tasks;
    using Common.Extensions;
    using Prism.Navigation;

    public class ViewModelBase : Common.ViewModels.ViewModelBase
    {
        public ViewModelBase(INavigationService navigationService) : base(navigationService)
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