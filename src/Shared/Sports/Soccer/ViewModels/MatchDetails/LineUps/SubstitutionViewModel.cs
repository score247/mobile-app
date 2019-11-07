namespace LiveScore.Soccer.ViewModels.MatchDetails.LineUps
{
    internal class SubstitutionViewModel : LineupsItemViewModel
    {
        public SubstitutionViewModel(string homeName,
            string awayName,
            int? homeJerseyNumber = null,
            int? awayJerseyNumber = null)
            : base(homeName, awayName, homeJerseyNumber, awayJerseyNumber, true)
        {
        }
    }
}