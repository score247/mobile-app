using System;
using System.Threading.Tasks;
using LiveScore.Common.Services;
using LiveScore.Core.Services;
using LiveScore.Soccer.Models.Odds;
using static LiveScore.Soccer.Services.SoccerApi;

namespace LiveScore.Soccer.Services
{
    public interface IOddsService
    {
        Task<MatchOdds> GetOddsAsync(
            string lang,
            string matchId,
            byte betTypeId,
            string formatType);

        Task<MatchOddsMovement> GetOddsMovementAsync(
            string lang,
            string matchId,
            byte betTypeId,
            string formatType,
            string bookmakerId);
    }

    public class OddsService : BaseService, IOddsService
    {
        private readonly IApiService apiService;
        private readonly OddsApi oddsApi;

        public OddsService(
            ILoggingService loggingService,
            IApiService apiService,
            OddsApi oddsApi = null) : base(loggingService)
        {
            this.apiService = apiService;
            this.oddsApi = oddsApi ?? apiService.GetApi<OddsApi>();
        }

        public async Task<MatchOdds> GetOddsAsync(
            string lang,
            string matchId,
            byte betTypeId,
            string formatType)
        {
            try
            {
                return await apiService.Execute(() => oddsApi.GetOdds(lang, matchId, betTypeId, formatType)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return null;
            }
        }

        public async Task<MatchOddsMovement> GetOddsMovementAsync(
            string lang,
            string matchId,
            byte betTypeId,
            string formatType,
            string bookmakerId)
        {
            try
            {
                return await apiService.Execute(() => oddsApi.GetOddsMovement(
                       lang,
                       matchId,
                       betTypeId,
                       formatType,
                       bookmakerId)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return null;
            }
        }
    }
}