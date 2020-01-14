using System;
using LiveScore.Common;
using LiveScore.Core;
using LiveScore.Core.Models.News;
using LiveScore.Common.Extensions;

namespace LiveScore.Features.News.ViewModels
{
    public class NewsItemViewModel
    {
        private readonly Func<string, string> buildNewsImageFunction;

        public NewsItemViewModel(INews news, IDependencyResolver dependencyResolver)
        {
            News = news;
            PublishedDate = news.PublishedDate.ToFullDateTime();
            Content = news.Content.Trim('\n');

            buildNewsImageFunction = dependencyResolver.Resolve<Func<string, string>>(FuncNameConstants.BuildNewsImageUrlFuncName);
        }

        public INews News { get; }

        public string Content { get; }

        public string PublishedDate { get; }

        public string NewsImageSource => buildNewsImageFunction(News.ImageName);
    }
}