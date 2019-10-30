namespace LiveScore.Soccer.ViewModels.MatchDetails.LineUps
{
    public class LineupsItemViewModel
    {
        public LineupsItemViewModel(
            string homeName,
            string awayName,
            int? homeJerseyNumber = null,
            int? awayJerseyNumber = null)
        {
            HomeName = homeName;
            AwayName = awayName;
            HomeJerseyNumber = homeJerseyNumber;
            AwayJerseyNumber = awayJerseyNumber;
        }

        public string HomeName { get; }

        public string AwayName { get; }

        public int? HomeJerseyNumber { get; }

        public int? AwayJerseyNumber { get; }
    }
}