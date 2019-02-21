namespace Score.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Score.Models;

    public interface IScoreService
    {
        IList<Match> GetAll(int sportId);
    }

    public class ScoreService : IScoreService
    {
        private int refreshTime = 0;

        public IList<Match> GetAll(int sportId)
        {
            var matches = GetMatches(sportId).ToList();

            for (int i = 1; i <= refreshTime; i++)
            {
                matches.AddRange(matches);
            }

            refreshTime++;
            return matches;
        }

        private IList<Match> GetMatches(int sportId)
        {
            switch (sportId)
            {
                case 1:
                    return GetSoccerMatches();

                case 2:
                    return GetTennisMatches();

                case 3:
                    return GetESportMatches();

                case 4:
                    return GetHockeyMatches();

                default:
                    return GetSoccerMatches();
            }
        }

        private IList<Match> GetSoccerMatches()
        {
            return new List<Match>
                {
                    new Match { HomeTeam = "Chelsea", AwayTeam = "Tottenham Hotspur", EventDate = DateTime.Today, GroupName = "ENGLAND EFL CUP" },
                    new Match { HomeTeam = "Fortuna Sittard", AwayTeam = "Vitesse", EventDate = DateTime.Today, GroupName = "NETHERLANDS EREDIVISIE" },
                    new Match { HomeTeam = "FC Utrecht", AwayTeam = "Willem II", EventDate = DateTime.Today, GroupName = "NETHERLANDS EREDIVISIE" },
                    new Match { HomeTeam = "Feyenoord", AwayTeam = "Ajax Amsterdam", EventDate = DateTime.Today, GroupName = "NETHERLANDS EREDIVISIE" },
                    new Match { HomeTeam = "SC Heerenveen", AwayTeam = "AZ Alkmaar", EventDate = DateTime.Today, GroupName = "NETHERLANDS EREDIVISIE" },
                    new Match { HomeTeam = "Verona", AwayTeam = "Cosenza", EventDate = DateTime.Today, GroupName = "ITALY SERIE C: GROUP A" },
                    new Match { HomeTeam = "Cararese Calcio 1908", AwayTeam = "As Lucchese Libertas 1905", EventDate = DateTime.Today, GroupName = "ITALY SERIE C: GROUP A" },
                    new Match { HomeTeam = "Olbia Calcio 1905", AwayTeam = "Virtus Entella", EventDate = DateTime.Today, GroupName = "ITALY SERIE C: GROUP A" },
                    new Match { HomeTeam = "Manchester United", AwayTeam = "Chelsea", EventDate = DateTime.Today, GroupName = "Champions League Group C" },
                    new Match { HomeTeam = "Manchester United", AwayTeam = "Chelsea", EventDate = DateTime.Today, GroupName = "Champions League Group C" },
                    new Match { HomeTeam = "Manchester United", AwayTeam = "Chelsea", EventDate = DateTime.Today, GroupName = "Champions League Group D" },
                    new Match { HomeTeam = "Manchester United", AwayTeam = "Chelsea", EventDate = DateTime.Today, GroupName = "Champions League Group D" },
                    new Match { HomeTeam = "Manchester United", AwayTeam = "Chelsea", EventDate = DateTime.Today, GroupName = "Champions League Group E" },
                    new Match { HomeTeam = "Manchester United", AwayTeam = "Chelsea", EventDate = DateTime.Today, GroupName = "Champions League Group E" },
                    new Match { HomeTeam = "Manchester United", AwayTeam = "Chelsea", EventDate = DateTime.Today, GroupName = "Champions League Group F" },
                    new Match { HomeTeam = "Manchester United", AwayTeam = "Chelsea", EventDate = DateTime.Today, GroupName = "Champions League Group F" },
                };
        }

        private IList<Match> GetTennisMatches()
        {
            return new List<Match>
                {
                    new Match { HomeTeam = "Roger Federer", AwayTeam = "Rafael Nadal", EventDate = DateTime.Today, GroupName = "Australia Open" },
                    new Match { HomeTeam = "Novak Djokovic", AwayTeam = "Lucas Pouille", EventDate = DateTime.Today, GroupName = "Australia Open" },
                    new Match { HomeTeam = "Naomi Osaka", AwayTeam = "Elina Svitolina", EventDate = DateTime.Today, GroupName = "Australia Open" },
                    new Match { HomeTeam = "Novak Djokovic", AwayTeam = "Rafael Nadal", EventDate = DateTime.Today, GroupName = "Wimbledon" },
                    new Match { HomeTeam = "John Isner", AwayTeam = "Kevin Anderson", EventDate = DateTime.Today, GroupName = "Wimbledon" },
                    new Match { HomeTeam = "Serena Williams", AwayTeam = "Camila Giorgi", EventDate = DateTime.Today, GroupName = "Wimbledon" },
                };
        }

        private IList<Match> GetESportMatches()
        {
            return new List<Match>
                {
                    new Match { HomeTeam = "Winners", AwayTeam = "MVP", EventDate = DateTime.Today, GroupName = "2019 Challengers Korea Spring" },
                    new Match { HomeTeam = "EGc", AwayTeam = "iG", EventDate = DateTime.Today, GroupName = "2019 Challengers Korea Spring" },
                    new Match { HomeTeam = "VSG", AwayTeam = "bbq", EventDate = DateTime.Today, GroupName = "2019 Challengers Korea Spring" },
                    new Match { HomeTeam = "Newbee", AwayTeam = "VP", EventDate = DateTime.Today, GroupName = "2019 MDL Macau" },
                    new Match { HomeTeam = "RNG", AwayTeam = "iG", EventDate = DateTime.Today, GroupName = "2019 MDL Macau" },
                    new Match { HomeTeam = "EHOME", AwayTeam = "EG", EventDate = DateTime.Today, GroupName = "2019 MDL Macau" },
                };
        }

        private IList<Match> GetHockeyMatches()
        {
            return new List<Match>
                {
                    new Match { HomeTeam = "Sweden", AwayTeam = "Canada", EventDate = DateTime.Today, GroupName = "World Championship" },
                    new Match { HomeTeam = "USA", AwayTeam = "Russia", EventDate = DateTime.Today, GroupName = "World Championship" },
                    new Match { HomeTeam = "France", AwayTeam = "Belarus", EventDate = DateTime.Today, GroupName = "World Championship" },
                    new Match { HomeTeam = "Froelunda HC", AwayTeam = "EHC Muenchen", EventDate = DateTime.Today, GroupName = "Champions League" },
                    new Match { HomeTeam = "Aalborg Pirates", AwayTeam = "Froelunda HC", EventDate = DateTime.Today, GroupName = "Champions League" },
                    new Match { HomeTeam = "Vienna Capitals", AwayTeam = "ZSC Lions", EventDate = DateTime.Today, GroupName = "Champions League" },
                };
        }
    }
}