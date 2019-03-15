namespace News.ViewModels
{
    using Common.ViewModels;
    using Prism.Navigation;

    public class NewsPageViewModel : ViewModelBase
    {
        public NewsPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }
    }
}