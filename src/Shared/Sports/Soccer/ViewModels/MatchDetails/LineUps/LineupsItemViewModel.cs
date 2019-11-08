using LiveScore.Core;
using LiveScore.Soccer.Views.Templates.MatchDetails.LineUps;
using Xamarin.Forms;

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

        public DataTemplate CreateTemplate()
        {
            if (IsSubstitution)
            {
                return new SubstitutionTemplate();
            }
            else
            {
                return new LineupsPlayerTemplate();
            }
        }
    }
}