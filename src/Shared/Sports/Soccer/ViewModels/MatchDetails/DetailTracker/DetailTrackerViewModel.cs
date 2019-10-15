using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Common.PlatformDependency;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.Models.Matches;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Services;
using Prism.Commands;
using Prism.Events;
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
        private readonly ISoccerMatchService matchInfoService;

        public DetailTrackerViewModel(
            MatchCoverage coverage,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator,
            DataTemplate dataTemplate)
            : base(navigationService, dependencyResolver, dataTemplate, eventAggregator)
        {
            matchCoverage = coverage;
            matchInfoService = dependencyResolver.Resolve<ISoccerMatchService>();
            OnCollapseTracker = new DelegateCommand(CollapseTracker);
            OnExpandTracker = new DelegateCommand(ExpandTracker);
            RefreshCommand = new DelegateAsyncCommand(OnRefresh);
        }

        public HtmlWebViewSource WidgetContent { get; set; }

        public ObservableCollection<CommentaryItemViewModel> MatchCommentaries { get; set; }

        public ObservableCollection<CommentaryItemViewModel> RemainingMatchCommentaries { get; set; }

        public bool TrackerVisible { get; private set; } = true;

        public bool TrackerHidden => !TrackerVisible;

        public bool HasTrackerData { get; private set; }

        public bool HasCommentariesData { get; private set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public bool IsRefreshing { get; set; }

        public DelegateCommand OnCollapseTracker { get; }

        public DelegateCommand OnExpandTracker { get; }

        public override Task OnNetworkReconnected()
            => LoadDataAsync(() => LoadMatchCommentaries(true));

        public override async void OnAppearing()
        {
            base.OnAppearing();

            await LoadTracker();
            await LoadDataAsync(() => LoadMatchCommentaries(true));
        }

        private void CollapseTracker() => TrackerVisible = false;

        private void ExpandTracker() => TrackerVisible = true;

        private Task OnRefresh() => LoadDataAsync(() => LoadMatchCommentaries(true), false);

        private async Task LoadTracker()
        {
            if (matchCoverage?.Coverage != null && matchCoverage.Coverage.Live)
            {
                WidgetContent = new HtmlWebViewSource
                {
                    Html = await GenerateTrackerWidget(matchCoverage.Coverage)
                };

                HasTrackerData = true;
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

        private async Task LoadMatchCommentaries(bool getLatestData = false)
        {
            var commentaries = (await matchInfoService
                    .GetMatchCommentaries(matchCoverage.MatchId, CurrentLanguage, getLatestData)).ToList();

            if (commentaries.Count > 0)
            {
                var commentaryViewModels = commentaries.Select(c => new CommentaryItemViewModel(c, DependencyResolver));

                MatchCommentaries = new ObservableCollection<CommentaryItemViewModel>(commentaryViewModels);
                SetFooterHeight(MatchCommentaries.Count);
                HasCommentariesData = true;
            }

            IsRefreshing = false;
        }
    }
}