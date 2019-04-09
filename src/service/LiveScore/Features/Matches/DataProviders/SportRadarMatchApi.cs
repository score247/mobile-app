namespace LiveScore.Features.Matches.DataProviders
{
    using System;
    using System.Threading.Tasks;
    using LiveScore.Shared.Models.Dtos;

    public class SportRadarMatchApi : IMatchApi
    {
        public Task<DailyScheduleDto> GetDailySchedules(int sportId, string lang, string date)
        {
            throw new NotImplementedException();
        }
    }
}