using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.ViewModels;
using Prism.Navigation;

namespace LiveScore.Features.News.ViewModels
{
    public class NewsViewModel : ViewModelBase
    {
        public NewsViewModel(INavigationService navigationService, IDependencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
            Title = AppResources.News;
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            if (!IsActive)
            {
                return;
            }
        }
    }
}