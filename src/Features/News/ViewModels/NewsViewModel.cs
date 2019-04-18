namespace LiveScore.News.ViewModels
{
    using Core.ViewModels;
    using LiveScore.Core.Factories;
    using Prism.Navigation;

    public class NewsViewModel : ViewModelBase
    {
        public NewsViewModel(INavigationService navigationService, IServiceLocator serviceLocator) : base(navigationService, serviceLocator)
        {
            Title = "News";
        }
    }
}