using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Services;
using LiveScore.Core.ViewModels;
using Prism.Navigation;

namespace LiveScore.Features.News.ViewModels
{
    public class NewsViewModel : ViewModelBase
    {
        private readonly INewsService newsService;

        public NewsViewModel(INavigationService navigationService, IDependencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
            Title = AppResources.News;
            TappedNewsCommand = new DelegateAsyncCommand<NewsItemViewModel>(OnTappedNews);
            RefreshCommand = new DelegateAsyncCommand(OnRefresh);
            newsService = DependencyResolver.Resolve<INewsService>(CurrentSportId.ToString());
        }

        public bool IsRefreshing { get; set; }

        public IReadOnlyList<NewsItemViewModel> NewsItemSource { get; private set; }

        public DelegateAsyncCommand<NewsItemViewModel> TappedNewsCommand { get; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            if (!IsActive)
            {
                return;
            }

            await LoadDataAsync(LoadNewsData);
        }

        private async Task OnRefresh()
        {
            await LoadDataAsync(LoadNewsData, false);

            IsRefreshing = false;
        }

        private async Task LoadNewsData()
        {
            var newsList = await newsService.GetNews(CurrentLanguage);

            NewsItemSource = new List<NewsItemViewModel>(newsList.Select(news => new NewsItemViewModel(news)));
        }

        private async Task OnTappedNews(NewsItemViewModel newsItem)
        {
            var parameters = new NavigationParameters
            {
                { "News", newsItem }
            };

            await NavigationService.NavigateAsync("NewsDetailView", parameters);
        }
    }
}