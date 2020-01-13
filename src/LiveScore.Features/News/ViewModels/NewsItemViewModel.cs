using LiveScore.Core.Models.News;

namespace LiveScore.Features.News.ViewModels
{
    public class NewsItemViewModel
    {
        public NewsItemViewModel(INews news)
        {
            News = news;
            PublishedDate = news.PublishedDate.ToString("dd MMM yyyy hh:mm").ToUpperInvariant();
            Content = news.Content.Trim('\n');
        }

        public INews News { get; }

        public string Content { get; }

        public string PublishedDate { get; }
    }
}