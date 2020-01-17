using System;
using System.Threading.Tasks;
using LiveScore.Common;
using LiveScore.Common.Extensions;
using LiveScore.Core;
using LiveScore.Core.Models.News;
using Prism.Navigation;

namespace LiveScore.Features.News.ViewModels
{
    public class NewsItemViewModel
    {
        private readonly Func<string, string> buildNewsImageFunction;

        public NewsItemViewModel(INews news, INavigationService navigationService, IDependencyResolver dependencyResolver)
        {
            News = news;
            TappedNewsCommand = new DelegateAsyncCommand(OnTappedNews);
            PublishedDate = news.PublishedDate.ToFullDateTime();
            Content = news.Content.Trim('\n');

            buildNewsImageFunction = dependencyResolver.Resolve<Func<string, string>>(FuncNameConstants.BuildNewsImageUrlFuncName);
            NavigationService = navigationService;
        }

        public INews News { get; }

        public string Content { get; }

        public string PublishedDate { get; }

        public string NewsImageSource => buildNewsImageFunction(News.ImageName);

        public DelegateAsyncCommand TappedNewsCommand { get; }

        public INavigationService NavigationService { get; protected set; }

        private async Task OnTappedNews()
        {
            var parameters = new NavigationParameters
            {
                { "News", this }
            };

            await NavigationService.NavigateAsync("NewsDetailView", parameters);
        }
    }
}