namespace LiveScore.Features.Matches.DataProviders
{
    using System.Threading.Tasks;
    using LiveScore.Shared.Models.Dtos;

    public interface IMatchApi
    {
        Task<DailyScheduleDto> GetDailySchedules(
            int sportId,
            string lang,
            string date);
    }
}