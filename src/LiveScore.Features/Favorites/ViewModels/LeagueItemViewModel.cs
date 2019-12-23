using LiveScore.Core.Models.Leagues;

namespace LiveScore.Features.Favorites.ViewModels
{
    public class LeagueItemViewModel
    {
        public LeagueItemViewModel(ILeague league, string countryFlag)
        {
            League = league;
            CountryFlag = countryFlag;
        }

        public ILeague League { get; }

        public string CountryFlag { get; }
    }
}
