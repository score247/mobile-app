using LiveScore.Core;

namespace LiveScore.Soccer.ViewModels.MatchDetails.LineUps
{
    public class LineupsItemViewModel
    {
        public LineupsItemViewModel(
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