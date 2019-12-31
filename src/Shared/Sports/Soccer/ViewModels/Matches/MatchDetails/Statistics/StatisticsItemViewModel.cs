namespace LiveScore.Soccer.ViewModels.Matches.MatchDetails.Statistics
{
    public class StatisticsItemViewModel
    {
        public StatisticsItemViewModel(
            string statisticName,
            byte? homeValue,
            byte? awayValue,
            bool isPossessionStatistic = false)
        {
            StatisticName = statisticName.ToUpperInvariant();
            HomeValue = homeValue ?? 0;
            AwayValue = awayValue ?? 0;
            IsHidden = HomeValue == 0 && AwayValue == 0;
            IsPossessionStatistic = isPossessionStatistic;

            if (IsVisibled)
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

        public bool IsPossessionStatistic { get; }

        public string HomePercentText => FormatPercent(HomePercent);

        public string AwayPercentText => FormatPercent(AwayPercent);

        private static string FormatPercent(float value)
            => value.ToString("P0", System.Globalization.CultureInfo.InvariantCulture).Replace(" ", string.Empty);
    }
}