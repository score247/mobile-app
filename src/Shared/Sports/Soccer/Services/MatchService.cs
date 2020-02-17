using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Common.Services;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Services;
using LiveScore.Soccer.Models.Matches;
using MethodTimer;
using static LiveScore.Soccer.Services.SoccerApi;

namespace LiveScore.Soccer.Services
{
    public interface ISoccerMatchService
    {
        Task<MatchInfo> GetMatchAsync(string matchId, Language language, DateTimeOffset eventDate);

        Task<IEnumerable<MatchCommentary>> GetMatchCommentariesAsync(string matchId, Language language, DateTimeOffset eventDate);

        Task<MatchStatistic> GetMatchStatisticAsync(string matchId, Language language, DateTimeOffset eventDate);

        Task<MatchLineups> GetMatchLineupsAsync(string matchId, Language language, DateTimeOffset eventDate);
    }

    public class MatchService : BaseService, IMatchService, ISoccerMatchService
    {
        private readonly IApiService apiService;
        private readonly MatchApi matchApi;

        public MatchService(
            IApiService apiService,
            ILoggingService loggingService,
            MatchApi matchApi = null) : base(loggingService)
        {
            this.apiService = apiService;
            this.matchApi = matchApi ?? apiService.GetApi<MatchApi>();
        }

        [Time]
        public async Task<IEnumerable<IMatch>> GetMatchesByDateAsync(DateTime dateTime, Language language)
        {
            try
            {
                return await apiService.Execute(() => matchApi.GetMatches(
                    dateTime.BeginningOfDay().ToApiFormat(),
                    dateTime.EndOfDay().ToApiFormat(),
                    language.DisplayName));
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Method", nameof(GetMatchAsync)},
                    { "FromDate", dateTime.BeginningOfDay().ToApiFormat()},
                    { "ToDate", dateTime.EndOfDay().ToApiFormat()}
                };

                HandleException(ex, properties);

                return Enumerable.Empty<IMatch>();
            }
        }

        [Time]
        public async Task<MatchInfo> GetMatchAsync(string matchId, Language language, DateTimeOffset eventDate)
        {
            try
            {
                return await apiService.Execute(() => matchApi.GetMatchInfo(matchId, language.DisplayName, eventDate.ToApiFormat()));
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Method", nameof(GetMatchAsync)},
                    { "MatchId", matchId}
                };

                HandleException(ex, properties);

                return new MatchInfo();
            }
        }

        public async Task<IEnumerable<IMatch>> GetLiveMatchesAsync(Language language)
        {
            try
            {
                return await apiService.Execute(() => matchApi.GetLiveMatches(language.DisplayName));
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Method", nameof(GetMatchAsync)}
                };


                HandleException(ex, properties);

                return Enumerable.Empty<IMatch>();
            }
        }

        public async Task<int> GetLiveMatchesCountAsync()
        {
            try
            {
                Debug.WriteLine("GetLiveMatchesCountAsync");
                var liveMatchCount = await apiService.Execute(() => matchApi.GetLiveMatchesCount(Language.English.DisplayName));

                return liveMatchCount;
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Method", nameof(GetLiveMatchesCountAsync)}
                };

                HandleException(ex, properties);

                return 0;
            }
        }

        public async Task<IEnumerable<IMatch>> GetMatchesByIds(string[] ids, Language language)
        {
            try
            {
                return await apiService.Execute(() => matchApi.GetMatchByIds(ids, language.DisplayName));
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Method", nameof(GetMatchesByIds)},
                    { "MatchIds", string.Join(';', ids)}
                };

                HandleException(ex, properties);

                return Enumerable.Empty<IMatch>();
            }
        }

        public async Task<IEnumerable<MatchCommentary>> GetMatchCommentariesAsync(string matchId, Language language, DateTimeOffset eventDate)
        {
            try
            {
                return await apiService.Execute(() => matchApi.GetMatchCommentaries(matchId, language.DisplayName, eventDate.ToApiFormat()));
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Method", nameof(GetMatchCommentariesAsync)},
                    { "MatchId", matchId}
                };

                HandleException(ex, properties);

                return Enumerable.Empty<MatchCommentary>();
            }
        }

        public async Task<MatchStatistic> GetMatchStatisticAsync(string matchId, Language language, DateTimeOffset eventDate)
        {
            try
            {
                return await apiService.Execute(() => matchApi.GetMatchStatistic(matchId, language.DisplayName, eventDate.ToApiFormat()));
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Method", nameof(GetMatchStatisticAsync)},
                    { "MatchId", matchId}
                };

                HandleException(ex, properties);

                return new MatchStatistic(matchId);
            }
        }

        public async Task<MatchLineups> GetMatchLineupsAsync(string matchId, Language language, DateTimeOffset eventDate)
        {
            try
            {
                return await apiService.Execute(() => matchApi.GetMatchLineups(matchId, language.DisplayName, eventDate.ToApiFormat()));
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Method", nameof(GetMatchStatisticAsync)},
                    { "MatchId", matchId}
                };
                
                HandleException(ex, properties);

                return new MatchLineups(matchId);
            }
        }
    }
}