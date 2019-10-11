namespace LiveScore.Soccer.Models.Matches
{
    using System.Collections.Generic;
    using System.Linq;
    using LiveScore.Common.LangResources;
    using LiveScore.Soccer.Models.Teams;
    using MessagePack;

    [MessagePackObject]
    public class MatchStatistic
    {
        public MatchStatistic(string matchId)
        {
            MatchId = matchId;
        }

        [SerializationConstructor]
        public MatchStatistic(
            string matchId,
            TeamStatistic homeStatistic,
            TeamStatistic awayStatistic)
        {
            MatchId = matchId;
            HomeStatistic = homeStatistic;
            AwayStatistic = awayStatistic;
        }

        [Key(0)]
        public string MatchId { get; }

        [Key(1)]
        public TeamStatistic HomeStatistic { get; }

        [Key(2)]
        public TeamStatistic AwayStatistic { get; }

        public MatchStatisticItem GetMainStatistic()
            => new MatchStatisticItem(AppResources.BallPossession, HomeStatistic?.Possession, AwayStatistic?.Possession);

#pragma warning disable S1541 // Methods and properties should not be too complex
        public IEnumerable<MatchStatisticItem> GetSubStatisticItems()
#pragma warning restore S1541 // Methods and properties should not be too complex
        {
            var subItems = new List<MatchStatisticItem>
            {
                new MatchStatisticItem(AppResources.ShotsOnTarget, HomeStatistic?.ShotsOnTarget, AwayStatistic?.ShotsOnTarget),
                new MatchStatisticItem(AppResources.ShotsOffTarget, HomeStatistic?.ShotsOffTarget, AwayStatistic?.ShotsOffTarget),
                new MatchStatisticItem(AppResources.BlockedShots, HomeStatistic?.ShotsBlocked, AwayStatistic?.ShotsBlocked),
                new MatchStatisticItem(AppResources.GoalKicks, HomeStatistic?.GoalKicks, AwayStatistic?.GoalKicks),
                new MatchStatisticItem(AppResources.CornerKicks, HomeStatistic?.CornerKicks, AwayStatistic?.CornerKicks),
                new MatchStatisticItem(AppResources.Offside, HomeStatistic?.Offsides, AwayStatistic?.Offsides),
                new MatchStatisticItem(AppResources.YellowCards, HomeStatistic?.YellowCards, AwayStatistic?.YellowCards),
                new MatchStatisticItem(AppResources.RedCards, HomeStatistic?.RedCards, AwayStatistic?.RedCards),
                new MatchStatisticItem(AppResources.ThrowIns, HomeStatistic?.ThrowIns, AwayStatistic?.ThrowIns),
                new MatchStatisticItem(AppResources.FreeKicks, HomeStatistic?.FreeKicks, AwayStatistic?.FreeKicks),
                new MatchStatisticItem(AppResources.Fouls, HomeStatistic?.Fouls, AwayStatistic?.Fouls),
            };

            if(subItems.All(item => item.IsHidden))
            {
                return Enumerable.Empty<MatchStatisticItem>();
            }

            return subItems;
        }
    }

    public class MatchStatisticItem
    {
        public MatchStatisticItem(string statisticName, byte? homeValue, byte? awayValue)
        {
            StatisticName = statisticName.ToUpperInvariant();
            HomeValue = homeValue.HasValue ? homeValue.Value : (byte)0;
            AwayValue = awayValue.HasValue ? awayValue.Value : (byte)0;
            IsHidden = HomeValue == 0 && AwayValue == 0; 

            if(IsVisibled)
            {
                var total = HomeValue + AwayValue;
                HomePercent = (float)HomeValue / total;
                AwayPercent = 1 - HomePercent;
            }
        }

        public string StatisticName { get; }

        public byte HomeValue { get; }

        public byte AwayValue { get; }

        public bool IsHidden { get; }

        public bool IsVisibled => !IsHidden;

        public float HomePercent { get; }

        public float AwayPercent { get; }
    }
        
}