namespace LiveScore.Soccer.ViewModels.MatchDetails.DetailLineups
{
    public class LineupsItemViewModel
    {
        public LineupsItemViewModel(string homeName, string awayName, int homeJerseyNumber = 0, int awayJerseyNumber = 0)
        {
            HomeName = homeName;
            AwayName = awayName;
            HomeJerseyNumber = homeJerseyNumber;
            AwayJerseyNumber = awayJerseyNumber;
        }

        public string HomeName { get; }

        public string AwayName { get; }

        public int HomeJerseyNumber { get; }

        public int AwayJerseyNumber { get; }
    }
}