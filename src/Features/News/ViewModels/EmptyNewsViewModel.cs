namespace LiveScore.News.ViewModels
{
    using LiveScore.Core;
    using LiveScore.Core.ViewModels;
    using Prism.Navigation;

    public class EmptyNewsViewModel : ViewModelBase
    {
        public EmptyNewsViewModel(INavigationService navigationService, IDepdendencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
            Title = "News";
        }
    }
}