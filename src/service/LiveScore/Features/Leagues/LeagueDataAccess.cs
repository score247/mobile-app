﻿namespace LiveScore.Features.Leagues
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

                var leagueRounds = BuildLeagueRound(matchByLeagueDtos, leagueRoundGroupDtos);

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

        private static List<LeagueRound> BuildLeagueRound(
            List<Result> matchByLeagueDtos, 
            IEnumerable<IGrouping<string, Result>> leagueRoundGroupDtos)
        {
            var leagueRounds = new List<LeagueRound>();

            foreach (var leagueRoundGroupDto in leagueRoundGroupDtos)
            {

                var leagueRoundDto = matchByLeagueDtos.FirstOrDefault().sport_event.tournament_round;

                var matches = BuildMatches(matchByLeagueDtos);

                var leagueRound = new LeagueRound
                {
                    Type = Enumeration.FromValue<LeagueRoundTypes>(leagueRoundDto.type),
                    Name = leagueRoundDto.name,
                    Number = leagueRoundDto.number,
                    Matches = matches
                };

                leagueRounds.Add(leagueRound);
            }

            return leagueRounds;
        }

        private static List<Match> BuildMatches(List<Result> matchByLeagueDtos)
        {
            var matches = new List<Match>();

            foreach (var matchDto in matchByLeagueDtos)
            {
                var sportEventDto = matchDto.sport_event;
                var sportEventStatusDto = matchDto.sport_event_status;
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