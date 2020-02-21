using LiveScore.Common.Extensions;
using LiveScore.Core.Models.Teams;
using LiveScore.Core.PubSubEvents.Teams;
using LiveScore.Soccer.Models.Teams;
using Newtonsoft.Json;

namespace LiveScore.Soccer.PubSubEvents.Teams
{
    public class TeamStatisticsMessage : ITeamStatisticsMessage
    {
        public TeamStatisticsMessage(
            byte sportId,
            string matchId,
            bool isHome,
            Core.Models.Teams.ITeamStatistic teamStatistic)
        {
            SportId = sportId;
            MatchId = matchId;
            IsHome = isHome;
            TeamStatistic = teamStatistic;
        }

        public byte SportId { get; private set; }

        public string MatchId { get; private set; }

        public bool IsHome { get; private set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<Models.Teams.TeamStatistic>))]
        public Core.Models.Teams.ITeamStatistic TeamStatistic { get; private set; }
    }
}