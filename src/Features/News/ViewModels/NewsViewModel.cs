namespace LiveScore.News.ViewModels
{
    using Core.ViewModels;
    using LiveScore.Core;
    using Prism.Navigation;

    public class NewsViewModel : ViewModelBase
    {
        public NewsViewModel(INavigationService navigationService, IDepdendencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
            Title = "News";
        }
    }
}