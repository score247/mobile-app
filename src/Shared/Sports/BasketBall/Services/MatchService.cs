namespace LiveScore.Basketball.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Models.Leagues;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Settings;
    using LiveScore.Core.Models.Teams;
    using LiveScore.Core.Services;

    public class MatchService : IMatchService
    {
        public async Task<IList<IMatch>> GetMatches(UserSettings settings, DateRange dateRange, bool forceFetchNewData = false)
        {
            var matches = new List<IMatch>
            {
                new Match {
                    EventDate = DateTime.Today,
                    League = new League { Name = "Basketball League" },
                    Teams = new List<Team> { new Team { Name = "Team A" }, new Team { Name = "Team B" } },
                    MatchResult = new MatchResult { HomeScore = 1, AwayScore = 2 }
                }
            };

            return matches;
        }
    }
}
