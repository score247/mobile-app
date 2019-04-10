namespace LiveScore.Soccers.Features.Matches.DataProviders
{
    using System.IO;
    using System.Threading.Tasks;
    using LiveScore.Shared.Configurations;
    using LiveScore.Shared.Models.Dtos;
    using Newtonsoft.Json;

    public class StaticMatchApi : IMatchApi
    {
        private readonly IAppSettings appSettings;

        public StaticMatchApi(IAppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        public Task<DailyScheduleDto> GetDailySchedules(
            int sportId,
            string lang,
            string date)
        {
            // Temporary for hardcode, please ignore it
            var jsonData = File.ReadAllText($"{appSettings.AppPath}/App_Data/staticdata/daily-schedule.json");

            return Task.FromResult(JsonConvert.DeserializeObject<DailyScheduleDto>(jsonData));
        }
    }
}