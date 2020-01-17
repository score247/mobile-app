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
        private bool firstLoad = true;

        public NewsViewModel(
            INavigationService navigationService,
            IDependencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
            Title = AppResources.News;
            RefreshCommand = new DelegateAsyncCommand(OnRefresh);
            newsService = DependencyResolver.Resolve<INewsService>(CurrentSportId.ToString());
        }

        public bool IsRefreshing { get; set; }

        public IReadOnlyList<NewsItemViewModel> NewsItemSource { get; private set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public override async void OnResumeWhenNetworkOK()
        {
            base.OnResumeWhenNetworkOK();

            await LoadDataAsync(LoadNewsData);
        }

        public override Task OnNetworkReconnectedAsync() => LoadDataAsync(LoadNewsData);

        public override async void OnAppearing()
        {
            base.OnAppearing();

            if (!IsActive || !firstLoad)
            {
                return;
            }

            await LoadDataAsync(LoadNewsData);

            firstLoad = false;
        }

        private async Task OnRefresh()
        {
            await LoadDataAsync(LoadNewsData, false);

            IsRefreshing = false;
        }

        private async Task LoadNewsData()
        {
            var newsList = (await newsService.GetNews(CurrentLanguage)).Take(30);

            NewsItemSource = new List<NewsItemViewModel>(newsList.Select(news => new NewsItemViewModel(news, NavigationService, DependencyResolver)));
        }
    }
}