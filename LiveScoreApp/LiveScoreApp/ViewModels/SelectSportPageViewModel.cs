namespace LiveScoreApp.ViewModels
{
    using Prism.Navigation;

    public class SelectSportPageViewModel : ViewModelBase
    {
        public SelectSportPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }
    }
}