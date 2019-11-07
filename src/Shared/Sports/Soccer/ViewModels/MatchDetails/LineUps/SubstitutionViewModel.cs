using LiveScore.Core;

namespace LiveScore.Soccer.ViewModels.MatchDetails.LineUps
{
    internal class SubstitutionViewModel : LineupsItemViewModel
    {
        public SubstitutionViewModel(
            IDependencyResolver dependencyResolver,
            string homeName,
            string awayName,
            int? homeJerseyNumber = null,
            int? awayJerseyNumber = null)
            : base(dependencyResolver, homeName, awayName, homeJerseyNumber, awayJerseyNumber, true)
        {
        }
    }
}