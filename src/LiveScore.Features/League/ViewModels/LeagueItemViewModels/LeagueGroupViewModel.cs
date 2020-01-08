using System;
using LiveScore.Core.Models.Leagues;

namespace LiveScore.Features.League.ViewModels.LeagueItemViewModels
{
    public class LeagueGroupViewModel
    {
        public LeagueGroupViewModel(string countryCode, string name, bool showFlag = false, Func<string, string> buildFlagFunction = null)
        {
            LeagueCategory = new LeagueCategory(countryCode, name);
            Name = name.ToUpperInvariant();
            ShowFlag = showFlag;
            CountryFlag = buildFlagFunction?.Invoke(countryCode);
        }

        public LeagueCategory LeagueCategory { get; }

        public string Name { get; }

        public bool ShowFlag { get; }

        public string CountryFlag { get; }

        public override bool Equals(object obj)
            => (obj is LeagueGroupViewModel actualObj) && Name == actualObj.Name;

        public override int GetHashCode()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return LeagueCategory?.CountryCode?.GetHashCode() ?? 0;
            }

            return Name.GetHashCode();
        }
    }
}