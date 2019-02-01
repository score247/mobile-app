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
                    new Match { HomeTeam = "Manchester United", AwayTeam = "Chelsea", EventDate = DateTime.Today, GroupName = "Champions League Round of 16" },
                    new Match { HomeTeam = "Manchester United", AwayTeam = "Chelsea", EventDate = DateTime.Today, GroupName = "Champions League Round of 16" },
                    new Match { HomeTeam = "Manchester United", AwayTeam = "Chelsea", EventDate = DateTime.Today, GroupName = "Champions League Round of 16" },
                    new Match { HomeTeam = "Manchester United", AwayTeam = "Chelsea", EventDate = DateTime.Today, GroupName = "Champions League Group A" },
                    new Match { HomeTeam = "Manchester United", AwayTeam = "Chelsea", EventDate = DateTime.Today, GroupName = "Champions League Group A" },
                    new Match { HomeTeam = "Manchester United", AwayTeam = "Chelsea", EventDate = DateTime.Today, GroupName = "Champions League Group A" },
                    new Match { HomeTeam = "Manchester United", AwayTeam = "Chelsea", EventDate = DateTime.Today, GroupName = "Champions League Group B" },
                    new Match { HomeTeam = "Manchester United", AwayTeam = "Chelsea", EventDate = DateTime.Today, GroupName = "Champions League Group B" },
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