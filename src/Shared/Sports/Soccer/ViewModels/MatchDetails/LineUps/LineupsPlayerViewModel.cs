namespace LiveScore.Soccer.ViewModels.MatchDetails.LineUps
{
    public class LineupsPlayerViewModel : LineupsItemViewModel
    {
        public LineupsPlayerViewModel(string homeName,
            string awayName,
            int? homeJerseyNumber = null,
            int? awayJerseyNumber = null)
            : base(homeName, awayName, homeJerseyNumber, awayJerseyNumber)
        {
        }
    }
}