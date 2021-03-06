using LiveScore.Core;

namespace LiveScore.Soccer.ViewModels.Matches.MatchDetails.LineUps
{
    public class LineupsItemViewModel
    {
        protected LineupsItemViewModel(
            IDependencyResolver dependencyResolver,
            bool isSubstitution = false)
        {
            DependencyResolver = dependencyResolver;
            IsSubstitution = isSubstitution;
        }

        public bool IsSubstitution { get; }

        public IDependencyResolver DependencyResolver { get; protected set; }
    }
}