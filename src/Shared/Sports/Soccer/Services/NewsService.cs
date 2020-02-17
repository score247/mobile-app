using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Services;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.News;
using LiveScore.Core.Services;

namespace LiveScore.Soccer.Services
{
    public class NewsService : BaseService, INewsService
    {
        private readonly IApiService apiService;
        private readonly SoccerApi.NewsApi newsApi;

        public NewsService(
            IApiService apiService,
            ILoggingService loggingService,
            SoccerApi.NewsApi newsApi = null) : base(loggingService)
        {
            this.apiService = apiService;
            this.newsApi = newsApi ?? apiService.GetApi<SoccerApi.NewsApi>();
        }

        public async Task<IEnumerable<INews>> GetNews(Language language)
        {
            try
            {
                return await apiService.Execute(() => newsApi.GetNews(language.DisplayName));
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                     { "Method", nameof(GetNews)}
                };

                HandleException(ex, properties);

                return Enumerable.Empty<INews>();
            }
        }
    }
}