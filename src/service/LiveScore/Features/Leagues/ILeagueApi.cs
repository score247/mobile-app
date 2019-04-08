﻿namespace LiveScore.Features.Leagues
{
    using System.Threading.Tasks;
    using LiveScore.Shared.Models.Dtos;
    using Refit;

    public interface ILeagueApi
    {
        [Get("/{sportId}-xt3/{group}/{lang}/schedules/{date}/results.json?api_key={key}")]
        Task<DailyScheduleDto> GetDailySchedules(string sportName, string group, string lang, string date, string key);
    }
}