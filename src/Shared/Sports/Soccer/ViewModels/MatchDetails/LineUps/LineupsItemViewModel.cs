using LiveScore.Soccer.Views.Templates.MatchDetails.LineUps;
using Xamarin.Forms;

namespace LiveScore.Soccer.ViewModels.MatchDetails.LineUps
{
    public class LineupsItemViewModel
    {
        public LineupsItemViewModel(
            string homeName,
            string awayName,
            int? homeJerseyNumber = null,
            int? awayJerseyNumber = null,
            bool isSubstitution = false)
        {
            HomeName = homeName;
            AwayName = awayName;
            HomeJerseyNumber = homeJerseyNumber;
            AwayJerseyNumber = awayJerseyNumber;
            IsSubstitution = isSubstitution;
        }

        public string HomeName { get; }

        public string AwayName { get; }

        public int? HomeJerseyNumber { get; }

        public int? AwayJerseyNumber { get; }
        public bool IsSubstitution { get; }

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