namespace League.Services
{
    using System;
    using System.Collections.Generic;
    using League.Models;

    public interface ILeagueService
    {
        IList<League> GetAll();

        IList<Match> GetLeagueMatches(string tournamentId);
    }

    internal class LeagueService : ILeagueService
    {
        public IList<League> GetAll()
        {
            return new List<League>
            {
                new League { Id = "1", Text = "Champions League", Description="This is an item description.", Image = "tab_feed.png" },
                new League { Id = "2", Text = "Europa League", Description="This is an item description.",Image = "tab_feed.png" },
                new League { Id = Guid.NewGuid().ToString(), Text = "Premiere League", Description="This is an item description.",Image = "tab_feed.png" },
                new League { Id = Guid.NewGuid().ToString(), Text = "Bundesliga", Description="This is an item description.",Image = "tab_feed.png" },
                new League { Id = Guid.NewGuid().ToString(), Text = "Laliga", Description="This is an item description.",Image = "tab_feed.png" },
                new League { Id = Guid.NewGuid().ToString(), Text = "V-League", Description="This is an item description.",Image = "tab_feed.png" },
                new League { Id = Guid.NewGuid().ToString(), Text = "V-League", Description="This is an item description.",Image = "tab_feed.png" },
                new League { Id = Guid.NewGuid().ToString(), Text = "V-League", Description="This is an item description.",Image = "tab_feed.png" },
                new League { Id = Guid.NewGuid().ToString(), Text = "V-League", Description="This is an item description.",Image = "tab_feed.png" },
                new League { Id = Guid.NewGuid().ToString(), Text = "V-League", Description="This is an item description.",Image = "tab_feed.png" },
                new League { Id = Guid.NewGuid().ToString(), Text = "V-League", Description="This is an item description.",Image = "tab_feed.png" },
                new League { Id = Guid.NewGuid().ToString(), Text = "V-League", Description="This is an item description.",Image = "tab_feed.png" },
                new League { Id = Guid.NewGuid().ToString(), Text = "V-League", Description="This is an item description.",Image = "tab_feed.png" },
            };
        }

        public IList<Match> GetLeagueMatches(string leagueId)
        {
            IList<Match> matches;

            if (leagueId == "1")
            {
                matches = new List<Match>
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
            else
            {
                matches = new List<Match>
                {
                    new Match { HomeTeam = "BATE Borisov", AwayTeam = "Arsenal", EventDate = DateTime.Today, GroupName = "Europa League Round of 16" },
                    new Match { HomeTeam = "BATE Borisov", AwayTeam = "Arsenal", EventDate = DateTime.Today, GroupName = "Europa League Round of 16" },
                    new Match { HomeTeam = "BATE Borisov", AwayTeam = "Arsenal", EventDate = DateTime.Today, GroupName = "Europa League Round of 16" },
                    new Match { HomeTeam = "BATE Borisov", AwayTeam = "Arsenal", EventDate = DateTime.Today, GroupName = "Europa League Group A" },
                    new Match { HomeTeam = "BATE Borisov", AwayTeam = "Arsenal", EventDate = DateTime.Today, GroupName = "Europa League Group A" },
                    new Match { HomeTeam = "BATE Borisov", AwayTeam = "Arsenal", EventDate = DateTime.Today, GroupName = "Europa League Group A" },
                    new Match { HomeTeam = "BATE Borisov", AwayTeam = "Arsenal", EventDate = DateTime.Today, GroupName = "Europa League Group B" },
                    new Match { HomeTeam = "BATE Borisov", AwayTeam = "Arsenal", EventDate = DateTime.Today, GroupName = "Europa League Group B" },
                };
            }

            return matches;
        }
    }
}