namespace LiveScore.Soccer.ViewModels.MatchDetails.DetailLineups
{
    public class LineupsItemViewModel
    {
        public LineupsItemViewModel(string homeValue, string awayValue)
        {
            HomeValue = homeValue;
            AwayValue = awayValue;
        }

        public string HomeValue { get; }

        public string AwayValue { get; }
    }
}