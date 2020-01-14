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
    }

    [MessagePackObject(keyAsPropertyName: true)]
    public class NewsItem : INews
    {
        [SerializationConstructor]
        public NewsItem(string title, string content, string imageName, string imageSource, string link, string author, DateTime publishedDate)
        {
            Title = title;
            Content = content;
            ImageName = imageName;
            ImageSource = imageSource;
            Link = link;
            Author = author;
            PublishedDate = publishedDate;
        }

        public string Title { get; }

        public string Content { get; }

        public string ImageName { get; }

        public string ImageSource { get; }

        public string Link { get; }

        public string Author { get; }

        public DateTime PublishedDate { get; }
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