namespace LiveScore.News.ViewModels
{
    using LiveScore.Core.Factories;
    using LiveScore.Core.ViewModels;
    using Prism.Navigation;

    public class EmptyNewsViewModel : ViewModelBase
    {
        public EmptyNewsViewModel(INavigationService navigationService, IServiceLocator serviceLocator) : base(navigationService, serviceLocator)
        {
            Title = "News";
        }
    }
}