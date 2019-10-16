using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.Models.Matches;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Services;
using MethodTimer;
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
        private readonly ISoccerMatchService soccerMatchService;
        private bool isFirstLoad = true;

        public DetailTrackerViewModel(
            MatchCoverage coverage,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator,
            DataTemplate dataTemplate)
            : base(navigationService, dependencyResolver, dataTemplate, eventAggregator)
        {
            matchCoverage = coverage;
            soccerMatchService = dependencyResolver.Resolve<ISoccerMatchService>();
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

        public override async void OnResumeWhenNetworkOK()
            => await LoadDataAsync(() => LoadMatchCommentariesAsync(true));

        public override Task OnNetworkReconnectedAsync()
            => LoadDataAsync(() => LoadMatchCommentariesAsync(true));

        public override async void OnAppearing()
        {
            base.OnAppearing();

            if (isFirstLoad)
            {
                LoadTracker();

                await LoadDataAsync(() => LoadMatchCommentariesAsync(true));
            }

            isFirstLoad = false;
        }

        private void CollapseTracker() => TrackerVisible = false;

        private void ExpandTracker() => TrackerVisible = true;

        private Task OnRefresh() => LoadDataAsync(() => LoadMatchCommentariesAsync(true), false);

        private void LoadTracker()
        {
            if (matchCoverage?.Coverage != null && matchCoverage.Coverage.Live)
            {
                WidgetContent = new HtmlWebViewSource
                {
                    Html = GenerateTrackerWidget(matchCoverage.Coverage)
                };

                HasTrackerData = true;
            }
        }

        private string GenerateTrackerWidget(Coverage coverage)
        {
            var formatMatchId = matchCoverage.MatchId.Replace(RemoveMatchPrefix, string.Empty);

            return TrackerWidgetHtml.Generate(new Dictionary<string, string>
            {
                { ReplacePrefix, formatMatchId },
                { WidgetPrefix, coverage.TrackerWidgetLink },
                { LanguagePrefix, LanguageCode },
            });

        }

        [Time]
        private async Task LoadMatchCommentariesAsync(bool forceFetchLatestData = false)
        {
            var commentaries = (await soccerMatchService
                    .GetMatchCommentariesAsync(matchCoverage.MatchId, CurrentLanguage, forceFetchLatestData))
                    .OrderByDescending(c => c.Time);

            if (commentaries.Any())
            {
                var commentaryViewModels = commentaries
                    .Select(c => new CommentaryItemViewModel(c, DependencyResolver));

                MatchCommentaries = new ObservableCollection<CommentaryItemViewModel>(commentaryViewModels);
                SetFooterHeight(MatchCommentaries.Count);
                HasCommentariesData = true;
            }

            SetHasData();
            IsRefreshing = false;
        }

        private void SetHasData()
        {
            HasData = HasTrackerData || HasCommentariesData;
        }
    }
}