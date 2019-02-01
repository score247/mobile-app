namespace Tournament.Services
{
    using System;
    using System.Collections.Generic;
    using Tournament.Models;

    public interface ITournamentService
    {
        IList<Tournament> GetAll();

        IList<Match> GetTournamentMatches(string tournamentId);
    }

    internal class TournamentService : ITournamentService
    {
        public IList<Tournament> GetAll()
        {
            return new List<Tournament>
            {
                new Tournament { Id = "1", Text = "Champions League", Description="This is an item description.", Image = "tab_feed.png" },
                new Tournament { Id = "2", Text = "Europa League", Description="This is an item description.",Image = "tab_feed.png" },
                new Tournament { Id = Guid.NewGuid().ToString(), Text = "Premiere League", Description="This is an item description.",Image = "tab_feed.png" },
                new Tournament { Id = Guid.NewGuid().ToString(), Text = "Bundesliga", Description="This is an item description.",Image = "tab_feed.png" },
                new Tournament { Id = Guid.NewGuid().ToString(), Text = "Laliga", Description="This is an item description.",Image = "tab_feed.png" },
                new Tournament { Id = Guid.NewGuid().ToString(), Text = "V-League", Description="This is an item description.",Image = "tab_feed.png" },
                new Tournament { Id = Guid.NewGuid().ToString(), Text = "V-League", Description="This is an item description.",Image = "tab_feed.png" },
                new Tournament { Id = Guid.NewGuid().ToString(), Text = "V-League", Description="This is an item description.",Image = "tab_feed.png" },
                new Tournament { Id = Guid.NewGuid().ToString(), Text = "V-League", Description="This is an item description.",Image = "tab_feed.png" },
                new Tournament { Id = Guid.NewGuid().ToString(), Text = "V-League", Description="This is an item description.",Image = "tab_feed.png" },
                new Tournament { Id = Guid.NewGuid().ToString(), Text = "V-League", Description="This is an item description.",Image = "tab_feed.png" },
                new Tournament { Id = Guid.NewGuid().ToString(), Text = "V-League", Description="This is an item description.",Image = "tab_feed.png" },
                new Tournament { Id = Guid.NewGuid().ToString(), Text = "V-League", Description="This is an item description.",Image = "tab_feed.png" },
            };
        }

        public IList<Match> GetTournamentMatches(string tournamentId)
        {
            IList<Match> matches;

            if (tournamentId == "1")
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