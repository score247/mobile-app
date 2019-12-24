using LiveScore.Core;
using LiveScore.Soccer.Models.Matches;

namespace LiveScore.Soccer.ViewModels.Matches.MatchDetails.LineUps
{
    public class SubstitutionViewModel : LineupsItemViewModel
    {
        public SubstitutionViewModel(
            IDependencyResolver dependencyResolver,
            TimelineEvent homeSubstitution,
            TimelineEvent awaySubstitution)
            : base(dependencyResolver, true)
        {
            BuildHomeSubstitution(homeSubstitution);
            BuildAwaySubstitution(awaySubstitution);
        }

        public bool IsHaveHomeSubsitution { get; private set; }

        public string HomeSubstitutionMinute { get; private set; }

        public string HomePlayerInJerseyNumber { get; private set; }

        public string HomePlayerInName { get; private set; }

        public string HomePlayerOutJerseyNumber { get; private set; }

        public string HomePlayerOutName { get; private set; }

        public bool IsHaveAwaySubsitution { get; private set; }

        public string AwaySubstitutionMinute { get; private set; }

        public string AwayPlayerInJerseyNumber { get; private set; }

        public string AwayPlayerInName { get; private set; }

        public string AwayPlayerOutJerseyNumber { get; private set; }

        public string AwayPlayerOutName { get; private set; }

        private void BuildHomeSubstitution(TimelineEvent homeSubstitution)
        {
            if (homeSubstitution != null)
            {
                IsHaveHomeSubsitution = true;
                HomeSubstitutionMinute = BuildMatchTime(homeSubstitution);
                HomePlayerInJerseyNumber = homeSubstitution.PlayerIn?.JerseyNumber.ToString();
                HomePlayerInName = homeSubstitution.PlayerIn?.Name;
                HomePlayerOutJerseyNumber = homeSubstitution.PlayerOut?.JerseyNumber.ToString();
                HomePlayerOutName = homeSubstitution.PlayerOut?.Name;
            }
        }

        private void BuildAwaySubstitution(TimelineEvent awaySubstitution)
        {
            if (awaySubstitution != null)
            {
                IsHaveAwaySubsitution = true;
                AwaySubstitutionMinute = BuildMatchTime(awaySubstitution);
                AwayPlayerInJerseyNumber = awaySubstitution.PlayerIn?.JerseyNumber.ToString();
                AwayPlayerInName = awaySubstitution.PlayerIn?.Name;
                AwayPlayerOutJerseyNumber = awaySubstitution.PlayerOut?.JerseyNumber.ToString();
                AwayPlayerOutName = awaySubstitution.PlayerOut?.Name;
            }
        }

        private static string BuildMatchTime(TimelineEvent timelineEvent)
        {
            return string.IsNullOrEmpty(timelineEvent.StoppageTime)
                ? $"{timelineEvent.MatchTime}'"
                : $"{timelineEvent.MatchTime}+{timelineEvent.StoppageTime}'";
        }
    }
}