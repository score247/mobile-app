using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.PlatformDependency;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.Models.Matches;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Services;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace LiveScore.Soccer.ViewModels.MatchDetails.DetailTracker
{
    internal class DetailTrackerViewModel : TabItemViewModel
    {
        private const string RemoveMatchPrefix = "sr:match:";
        private const string ReplacePrefix = "input-match-id";
        private const string WidgetPrefix = "widget-url";
        private const string LanguagePrefix = "input-language";
        private const string LanguageCode = "en";

        private readonly MatchCoverage matchCoverage;
        private readonly IMatchInfoService matchInfoService;

        public DetailTrackerViewModel(
            MatchCoverage coverage,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            DataTemplate dataTemplate)
            : base(navigationService, dependencyResolver, dataTemplate)
        {
            matchCoverage = coverage;
            matchInfoService = dependencyResolver.Resolve<IMatchInfoService>();
            OnCollapseTracker = new DelegateCommand(CollapseTracker);
            OnExpandTracker = new DelegateCommand(ExpandTracker);
        }

        public HtmlWebViewSource WidgetContent { get; set; }

        public ObservableCollection<CommentaryItemViewModel> MatchCommentaries { get; set; }

        public ObservableCollection<CommentaryItemViewModel> RemainingMatchCommentaries { get; set; }

        public bool TrackerVisible { get; private set; } = true;

        public bool TrackerHidden => !TrackerVisible;

        public bool HasTrackerData { get; private set; }

        public bool HasCommentariesData { get; private set; }

        public DelegateCommand OnCollapseTracker { get; }

        public DelegateCommand OnExpandTracker { get; }

        public override Task OnNetworkReconnected()
        {
            OnAppearing();

            return Task.CompletedTask;
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            await LoadMatchCommentaries(true);

            if (matchCoverage?.Coverage != null && matchCoverage.Coverage.Live)
            {
                WidgetContent = new HtmlWebViewSource
                {
                    Html = await GenerateTrackerWidget(matchCoverage.Coverage)
                };

                HasTrackerData = true;
                HasData = true;
            }

            if (matchCoverage?.Coverage?.Commentary == true)
            {
                await LoadMatchCommentaries(true);
            }
            else
            {
                HasData = false;
            }
        }

        private async Task<string> GenerateTrackerWidget(Coverage coverage)
        {
            var formatMatchId = matchCoverage.MatchId.Replace(RemoveMatchPrefix, string.Empty);

            var content = await File.ReadAllTextAsync(DependencyService.Get<IBaseUrl>().Get() + "/html/TrackerWidget.html");

            content = content.Replace(ReplacePrefix, formatMatchId);
            content = content.Replace(WidgetPrefix, coverage.TrackerWidgetLink);
            content = content.Replace(LanguagePrefix, LanguageCode);

            return content;
        }

        private void CollapseTracker() => TrackerVisible = false;

        private void ExpandTracker() => TrackerVisible = true;

        private async Task LoadMatchCommentaries(bool getLatestData = false)
        {
            var commentaries = await matchInfoService.GetMatchCommentaries(matchCoverage.MatchId, CurrentLanguage, getLatestData);
            var commentaryViewModels = commentaries.Select(c => new CommentaryItemViewModel(c, DependencyResolver));

            MatchCommentaries = new ObservableCollection<CommentaryItemViewModel>(commentaryViewModels);
        }
    }
}