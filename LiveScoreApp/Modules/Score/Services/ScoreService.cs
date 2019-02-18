namespace Score.Services
{
    using System;
    using System.Collections.Generic;
    using Score.Models;

    public interface IScoreService
    {
        IList<Match> GetAll();
    }

    public class ScoreService : IScoreService
    {
        public IList<Match> GetAll()
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
    }
}