using System;
using System.Collections.Generic;
using MessagePack;

namespace LiveScore.Core.Models.News
{
    public interface INews
    {
        string Title { get; }

        string Content { get; }

        string ImageName { get; }

        string ImageSource { get; }

        string Link { get; }

        string Author { get; }

        DateTime PublishedDate { get; }

        string Provider { get; }
    }

    [MessagePackObject(keyAsPropertyName: true)]
    public class NewsItem : INews
    {
        [SerializationConstructor]
#pragma warning disable S107 // Methods should not have too many parameters
        public NewsItem(string title, string content, string imageName, string imageSource, string link, string author, DateTime publishedDate, string provider)
        {
            Title = title;
            Content = content;
            ImageName = imageName;
            ImageSource = imageSource;
            Link = link;
            Author = author;
            PublishedDate = publishedDate;
            Provider = provider;
        }
#pragma warning restore S107 // Methods should not have too many parameters

        public string Title { get; }

        public string Content { get; }

        public string ImageName { get; }

        public string ImageSource { get; }

        public string Link { get; }

        public string Author { get; }

        public DateTime PublishedDate { get; }

        public string Provider { get; }
    }

    /// <summary>
    /// Temp class for Message Pack generate AOT class
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public class NewsItemList
    {
        public IEnumerable<NewsItem> NewsItems { get; set; }
    }
}