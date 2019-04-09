namespace LiveScore.Features.Matches
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using LiveScore.Features.Leagues.Models;
    using LiveScore.Features.Matches.DataProviders;
    using LiveScore.Features.Matches.Models;
    using LiveScore.Shared.Models;
    using LiveScore.Shared.Models.Dtos;

    public interface MatchDataAccess
    {
        Task<IEnumerable<Match>> GetMatches(int sportId, DateTime date, string language);
    }

    public class MatchDataAccessImpl : MatchDataAccess
    {
        private const string Home = "home";
        private readonly IMatchApi matchApi;

        public MatchDataAccessImpl(IMatchApi matchApi)
        {
            this.matchApi = matchApi;
        }

        public async Task<IEnumerable<Match>> GetMatches(int sportId, DateTime date, string language)
        {
            var dailySchedule = await matchApi.GetDailySchedules(
                sportId,
                language,
                DateTime.Now.ToShortDateString());

            var matches = new List<Match>();

            foreach (var matchDto in dailySchedule.results)
            {
                var sportEventDto = matchDto.sport_event;
                var sportEventStatusDto = matchDto.sport_event_status;

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
                    Teams = BuildTeams(sportEventDto),
                    League = BuildLeague(matchDto.sport_event.tournament),
                    LeagueRound = BuildLeagueRound(matchDto.sport_event.tournament_round)
                });
            }

            return matches;
        }

        private static LeagueRound BuildLeagueRound(Tournament_Round leagueRoundDto)
        {
            return new LeagueRound
            {
                Type = Enumeration.FromValue<LeagueRoundTypes>(leagueRoundDto.type),
                Name = leagueRoundDto.name,
                Number = leagueRoundDto.number,
                Phase = leagueRoundDto.phase
            };
        }

        private static League BuildLeague(
            Tournament leagueDto)
        {
            var leagueCategoryDto = leagueDto.category;

            return new League
            {
                Id = leagueDto.id,
                Name = leagueDto.name,
                Category = new LeagueCategory
                {
                    Id = leagueCategoryDto?.id,
                    Name = leagueCategoryDto?.name,
                    CountryCode = leagueCategoryDto?.country_code
                }
            };
        }

        private static List<Teams.Models.Team> BuildTeams(Sport_Event sportEventDto)
        {
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
                    IsHome = string.Compare(competitor.qualifier, Home, true, CultureInfo.InvariantCulture) == 0
                });
            }

            return teams;
        }
    }
}