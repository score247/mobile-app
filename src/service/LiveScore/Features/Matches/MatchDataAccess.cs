namespace LiveScore.Features.Matches
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiveScore.Domain.Enumerations;
    using LiveScore.Domain.Models.Matches;

    public interface MatchDataAccess
    {
        Task<IEnumerable<Match>> GetMatches(int sportId, DateTime date);
    }

    public class MatchDataAccessImpl : MatchDataAccess
    {
        private readonly IMatchApi matchApi;

        public MatchDataAccessImpl(IMatchApi matchApi)
        {
            this.matchApi = matchApi;
        }

        public async Task<IEnumerable<Match>> GetMatches(int sportId, DateTime date)
        {
            var dailySchedule = await matchApi.GetDailySchedules(
                "soccer",
                "eu",
                "en",
                DateTime.Now.ToString(),
                "key");

            var matches = new List<Match>();

            foreach (var matchDto in dailySchedule.results)
            {
                var sportEventDto = matchDto.sport_event;
                var sportEventStatusDto = matchDto.sport_event_status;
                var leagueDto = matchDto.sport_event.tournament;
                var leagueRoundDto = matchDto.sport_event.tournament_round;
                var teams = new List<Domain.Models.Teams.Team>();

                foreach (var competitor in sportEventDto.competitors)
                {
                    teams.Add(new Domain.Models.Teams.Team
                    {
                        Id = competitor.id,
                        Country = competitor.country,
                        CountryCode = competitor.country_code,
                        Name = competitor.name,
                        Abbreviation = competitor.abbreviation,
                        IsHome = string.Compare(competitor.qualifier, "home", true) == 0
                    });
                }

                matches.Add(new Match
                {
                    Id = sportEventDto.id,
                    EventDate = sportEventDto.scheduled,
                    LeagueId = leagueDto?.id,
                    LeagueName = leagueDto?.name,
                    LeagueRoundName = leagueRoundDto?.type,
                    LeagueCategory = leagueDto?.category?.name,
                    MatchResult = new MatchResult
                    {
                        Status = Enumeration.FromValue<MatchStatus>(sportEventStatusDto.status),
                        HomeScores = new List<int> { sportEventStatusDto.home_score },
                        AwayScores = new List<int> { sportEventStatusDto.away_score },
                    },
                    Teams = teams
                });
            }

            return matches;
        }
    }
}