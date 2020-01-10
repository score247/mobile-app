using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.News;

namespace LiveScore.Core.Services
{
    public interface INewsService
    {
        Task<IEnumerable<INews>> GetNews(Language language);
    }
}