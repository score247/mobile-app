namespace LiveScore.Features.Matches
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiveScore.Features.Leagues.Models;
    using LiveScore.Features.Matches.Models;
    using LiveScore.Shared.Models;

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
                var teams = new List<Teams.Models.Team>();

                foreach (var competitor in sportEventDto.competitors)
                {
                    teams.Add(new Teams.Models.Team
                    {
                        Id = competitor.id,
                        Country = competitor.country,
                        CountryCode = competitor.country_code,
                        Name = competitor.name,
                        Abbreviation = competitor.abbreviation,
                        IsHome = string.Compare(competitor.qualifier, "home", true) == 0
                    });
                }

                var leagueRound = new LeagueRound
                {
                    Type = Enumeration.FromValue<LeagueRoundTypes>(leagueRoundDto.type),
                    Name = leagueRoundDto.name,
                    Number = leagueRoundDto.number,
                    Phase = leagueRoundDto.phase
                };

                var leagueCategoryDto = leagueDto.category;

                matches.Add(new Match
                {
                    Id = sportEventDto.id,
                    EventDate = sportEventDto.scheduled,
                    MatchResult = new MatchResult
                    {
                        Status = Enumeration.FromValue<MatchStatus>(sportEventStatusDto.status),
                        HomeScores = new List<int> { sportEventStatusDto.home_score },
                        AwayScores = new List<int> { sportEventStatusDto.away_score },
                    },
                    Teams = teams,
                    League = new League
                    {
                        Id = leagueDto.id,
                        Name = leagueDto.name,
                        Category = new LeagueCategory
                        {
                            Id = leagueCategoryDto?.id,
                            Name = leagueCategoryDto?.name,
                            CountryCode = leagueCategoryDto?.country_code
                        }
                    },
                    LeagueRound = leagueRound
                });
            }

            return matches;
        }
    }
}