using LiveScore.Core.Models.Leagues;

namespace LiveScore.Core.Extensions
{
    public static class LeagueExtension
    {
        public static string CombineCountryName(this ILeague league)
               => league.IsInternational ? league.Name : $"{league.CountryName} {league.Name}";
    }
}