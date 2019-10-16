using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
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
        private const int DefaultLoadingCommentaryItemCount = 30;
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
            : base(navigationService, dependencyResolver, dataTemplate, eventAggregator, 9)
        {
            matchCoverage = coverage;
            soccerMatchService = dependencyResolver.Resolve<ISoccerMatchService>();
            OnCollapseTracker = new DelegateCommand(CollapseTracker);
            OnExpandTracker = new DelegateCommand(ExpandTracker);
            RefreshCommand = new DelegateAsyncCommand(OnRefresh);
            ShowMoreCommentariesCommand = new DelegateCommand(ShowMoreCommentaries);
            IsBusy = true;
        }

        public HtmlWebViewSource WidgetContent { get; set; }

        public ObservableCollection<CommentaryItemViewModel> MatchCommentaries { get; set; }

        public IEnumerable<CommentaryItemViewModel> DefaultMatchCommentaries { get; set; }

        public IEnumerable<CommentaryItemViewModel> RemainingMatchCommentaries { get; set; }

        public bool TrackerVisible { get; private set; } = true;

        public bool TrackerHidden => !TrackerVisible;

        public bool HasTrackerData { get; private set; }

        public bool HasCommentariesData { get; private set; }

        public bool IsRefreshing { get; set; }

        public bool IsShowMore { get; private set; } = true;

        public bool IsShowLess => !IsShowMore;

        public string ShowLessMoreButtonText { get; private set; } = AppResources.ShowMore;

        public DelegateAsyncCommand RefreshCommand { get; }

        public DelegateCommand OnCollapseTracker { get; }

        public DelegateCommand OnExpandTracker { get; }

        public DelegateCommand ShowMoreCommentariesCommand { get; }

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

        private void ShowMoreCommentaries()
        {
            if (IsShowMore)
            {
                foreach (var commentary in RemainingMatchCommentaries)
                {
                    MatchCommentaries.Add(commentary);
                }

                ShowLessMoreButtonText = AppResources.ShowLess;
                IsShowMore = false;
            }
            else
            {
                MatchCommentaries = new ObservableCollection<CommentaryItemViewModel>(DefaultMatchCommentaries);
                ShowLessMoreButtonText = AppResources.ShowMore;
                IsShowMore = true;
            }
        }

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
                    .OrderByDescending(c => c.Time)
                    .ToList();

            if (commentaries.Count > 0)
            {
                var commentaryViewModels = commentaries
                    .Select(c => new CommentaryItemViewModel(c, DependencyResolver))
                    .ToList();

                DefaultMatchCommentaries = commentaryViewModels.Take(DefaultLoadingCommentaryItemCount);
                RemainingMatchCommentaries = commentaryViewModels.Skip(DefaultLoadingCommentaryItemCount);
                MatchCommentaries = new ObservableCollection<CommentaryItemViewModel>(DefaultMatchCommentaries);

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