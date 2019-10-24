using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

namespace LiveScore.Soccer.ViewModels.DetailH2H
{
    public class DetailH2HViewModel : TabItemViewModel
    {
        private readonly string matchId;

        public DetailH2HViewModel(
            string matchId,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator,
            DataTemplate dataTemplate)
            : base(navigationService, dependencyResolver, dataTemplate, eventAggregator, AppResources.H2H)
        {
            this.matchId = matchId;

            VisibleHeadToHead = true;

            OnTeamResultTapped = new DelegateCommand<string>(teamIdentifier
                => LoadTeamResult(teamIdentifier));

            OnHeadToHeadTapped = new DelegateCommand(LoadHeadToHead);
        }

        public bool VisibleHomeResults { get; private set; }

        public bool VisibleAwayResults { get; private set; }

        public bool VisibleHeadToHead { get; private set; }

        public DelegateCommand<string> OnTeamResultTapped { get; }

        public DelegateCommand OnHeadToHeadTapped { get; }
        

        private void LoadTeamResult(string teamIdentifier)
        {
            if (teamIdentifier == "home")
            {
                VisibleHomeResults = true;
                VisibleAwayResults = false;
            }
            else
            {
                VisibleHomeResults = false;
                VisibleAwayResults = true;
            }

            VisibleHeadToHead = false;
        }

        private void LoadHeadToHead()
        {
            VisibleHeadToHead = true;

            VisibleHomeResults = false;
            VisibleAwayResults = false;
        }
    }
}