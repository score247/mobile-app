namespace LiveScore.Features.Leagues
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiveScore.Domain.Models.Leagues;
    using LiveScore.Shared.Models.Dtos;
    using LiveScore.Domain.Enumerations;
    using LiveScore.Domain.Models.Matches;

    public interface LeagueDataAccess
    {
        Task<IEnumerable<League>> GetLeagues(int sportId, DateTime date);
    }

    public class LeagueDataAccessImpl : LeagueDataAccess
    {
        private readonly ILeagueApi leagueApi;

        public LeagueDataAccessImpl(ILeagueApi leagueApi)
        {
            this.leagueApi = leagueApi;
        }

        public async Task<IEnumerable<League>> GetLeagues(int sportId, DateTime date)
        {
            var dailySchedule = await leagueApi.GetDailySchedules(
                "soccer", 
                "eu", 
                "us", 
                DateTime.Now.ToString(), 
                "key");

            var leagues = new List<League>();

            var leagueDtoGroups = dailySchedule
                .results
                .Where(result => result?.sport_event?.tournament != null)
                .GroupBy(result => result.sport_event.tournament.id);

            foreach (var leagueDtoGroup in leagueDtoGroups)
            {
                var matchByLeagueDtos = leagueDtoGroup.ToList();
                var leagueDto = matchByLeagueDtos.FirstOrDefault().sport_event.tournament;
                var leagueCategoryDto = leagueDto.category;

                var leagueRoundGroupDtos = matchByLeagueDtos
                    .Where(result => result?.sport_event?.tournament_round != null)
                    .GroupBy(result =>
                    string.Join(
                        "_",
                        result.sport_event.tournament_round.type,
                        result.sport_event.tournament_round.number));
                var leagueRounds = new List<LeagueRound>();

                foreach (var leagueRoundGroupDto in leagueRoundGroupDtos)
                {
                    
                    var leagueRoundDto = matchByLeagueDtos.FirstOrDefault().sport_event.tournament_round;

                    var leagueRound = new LeagueRound
                    {
                        Type = Enumeration.FromValue<LeagueRoundTypes>(leagueRoundDto.type),
                        Name = leagueRoundDto.name,
                        Number = leagueRoundDto.number,
                        Matches = matchByLeagueDtos.Select(matchDto =>
                        {
                            var sportEventDto = matchDto.sport_event;
                            var sportEventStatusDto = matchDto.sport_event_status;

                            return new Match
                            {
                                Id = sportEventDto.id,
                                EventDate = sportEventDto.scheduled,
                                MatchResult = new MatchResult
                                {
                                    Status = Enumeration.FromValue<MatchStatus>(sportEventStatusDto.status),
                                    HomeScores = new List<int> { sportEventStatusDto.home_score },
                                    AwayScores = new List<int> { sportEventStatusDto.away_score },
                                },
                                Teams = sportEventDto.competitors.Select(competitor
                                    => new Domain.Models.Teams.Team
                                    {
                                        Id = competitor.id,
                                        Country = competitor.country,
                                        CountryCode = competitor.country_code,
                                        Name = competitor.name,
                                        Abbreviation = competitor.abbreviation,
                                        IsHome = string.Compare(competitor.qualifier, "home", true) == 0
                                    })
                            };
                        })
                    };

                    leagueRounds.Add(leagueRound);
                }

                

                var league = new League
                {
                    Id = leagueDto.id,
                    Name = leagueDto.name,
                    Category = new LeagueCategory
                    {
                        Id = leagueCategoryDto?.id,
                        Name = leagueCategoryDto?.name
                    },
                    Rounds = leagueRounds
                };

                leagues.Add(league);
            }

            return leagues;
        }
    }
}