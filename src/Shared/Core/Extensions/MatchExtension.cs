using System;
using LiveScore.Core.Models.Matches;

namespace LiveScore.Core.Extensions
{
    public static class MatchExtension
    {
        public static bool IsEnableFavorite(this IMatch match)
            => match.EventDate >= DateTime.Today.AddDays(-2);
    }
}