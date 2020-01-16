using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.ViewModels;
using Prism.Navigation;

namespace LiveScore.Features.News.ViewModels
{
    public class NewsDetailViewModel : ViewModelBase
    {
        public NewsDetailViewModel(INavigationService navigationService, IDependencyResolver serviceLocator)
            : base(navigationService, serviceLocator)
        {
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters["News"] is NewsItemViewModel newsItem)
            {
                NewsItem = newsItem;
                Author = string.IsNullOrWhiteSpace(newsItem.News.Author) ? string.Empty : $"{AppResources.By} {newsItem.News.Author?.ToUpperInvariant()}";
            }
        }

        public NewsItemViewModel NewsItem { get; private set; }

        public string Author { get; private set; }
    }
}