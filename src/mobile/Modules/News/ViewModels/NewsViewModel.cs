namespace News.ViewModels
{
    using Common.ViewModels;
    using Prism.Navigation;

    public class NewsViewModel : ViewModelBase
    {
        public NewsViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }
    }
}