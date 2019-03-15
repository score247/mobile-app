using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace Setting.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; }
        public DelegateCommand DoneCommand { get; set; }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
            DoneCommand = new DelegateCommand(OnDone);
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {
        }

        public virtual void Destroy()
        {
        }

        protected virtual async void OnDone()
        {
            await NavigationService.GoBackAsync(useModalNavigation: true);
        }
    }
}