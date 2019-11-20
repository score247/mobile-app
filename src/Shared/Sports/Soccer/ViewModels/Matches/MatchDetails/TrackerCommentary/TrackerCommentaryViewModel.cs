﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.Models.Matches;
using LiveScore.Soccer.Extensions;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Services;
using MethodTimer;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

[assembly: InternalsVisibleTo("Soccer.Tests")]
namespace LiveScore.Soccer.ViewModels.MatchDetails.TrackerCommentary
{
    public class TrackerCommentaryViewModel : TabItemViewModel
    {
        private const int DefaultLoadingCommentaryItemCount = 30;
        private const string RemoveMatchPrefix = "sr:match:";
        private const string ReplacePrefix = "input-match-id";
        private const string WidgetPrefix = "widget-url";
        private const string LanguagePrefix = "input-language";
        private const string LanguageCode = "en";
        private readonly MatchCoverage matchCoverage;
        private readonly ISoccerMatchService soccerMatchService;

        public TrackerCommentaryViewModel(
            MatchCoverage matchCoverage,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator,
            DataTemplate dataTemplate)
            : base(navigationService, dependencyResolver, dataTemplate, eventAggregator, AppResources.Tracker)
        {
            this.matchCoverage = matchCoverage;
            soccerMatchService = dependencyResolver.Resolve<ISoccerMatchService>();
            OnCollapseTracker = new DelegateCommand(() => TrackerVisible = false);
            OnExpandTracker = new DelegateCommand(() => TrackerVisible = true);
            RefreshCommand = new DelegateAsyncCommand(OnRefresh);
            ShowMoreCommentariesCommand = new DelegateCommand(ShowMoreCommentaries);
        }

        public HtmlWebViewSource WidgetContent { get; set; }

        public ObservableCollection<CommentaryItemViewModel> MatchCommentaries { get; set; }

        public IEnumerable<CommentaryItemViewModel> DefaultMatchCommentaries { get; set; }

        public IEnumerable<CommentaryItemViewModel> FullMatchCommentaries { get; set; }

        public bool TrackerVisible { get; private set; } = true;

        public bool TrackerHidden => !TrackerVisible;

        public bool HasTrackerData { get; private set; }

        public bool HasCommentariesData { get; private set; }

        public bool IsRefreshing { get; set; }

        public bool IsShowMore { get; private set; } = true;

        public bool IsShowLess => !IsShowMore;

        public string ShowLessMoreButtonText { get; private set; } = AppResources.ShowMore;

        public bool VisibleShowMore { get; private set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public DelegateCommand OnCollapseTracker { get; }

        public DelegateCommand OnExpandTracker { get; }

        public DelegateCommand ShowMoreCommentariesCommand { get; }

        public override async void OnResumeWhenNetworkOK()
            => await LoadDataAsync(LoadMatchCommentariesAsync);

        public override Task OnNetworkReconnectedAsync()
            => LoadDataAsync(LoadMatchCommentariesAsync);

        public override async void OnAppearing()
        {
            base.OnAppearing();

            await LoadDataAsync(LoadTrackerAndCommentaries).ConfigureAwait(false);
        }

        private Task OnRefresh()
            => LoadDataAsync(LoadMatchCommentariesAsync, false);

        internal void ShowMoreCommentaries()
        {
            if (IsShowMore)
            {
                MatchCommentaries = new ObservableCollection<CommentaryItemViewModel>(FullMatchCommentaries);
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

        internal async Task LoadTrackerAndCommentaries()
        {
            if (matchCoverage?.Coverage == null)
            {
                HasData = false;
                return;
            }

            if (IsFirstLoad)
            {
                LoadTracker();
                IsFirstLoad = false;
            }

            await LoadDataAsync(LoadMatchCommentariesAsync);
        }

        private void LoadTracker()
        {
            if (matchCoverage?.Coverage == null || !matchCoverage.Coverage.Live)
            {
                return;
            }

            WidgetContent = new HtmlWebViewSource
            {
                Html = GenerateTrackerWidget(matchCoverage.Coverage)
            };

            HasTrackerData = true;
        }

        private string GenerateTrackerWidget(Coverage coverage)
        {
            var formatMatchId = matchCoverage?.MatchId.Replace(RemoveMatchPrefix, string.Empty);

            return TrackerWidgetHtml.Generate(new Dictionary<string, string>
            {
                { ReplacePrefix, formatMatchId },
                { WidgetPrefix, coverage.TrackerWidgetLink },
                { LanguagePrefix, LanguageCode },
            });
        }

        [Time]
        private async Task LoadMatchCommentariesAsync()
        {
            if (matchCoverage?.Coverage != null && matchCoverage.Coverage.Commentary)
            {
                var matchCommentaries = (await soccerMatchService
                        .GetMatchCommentariesAsync(matchCoverage.MatchId, CurrentLanguage))
                        .Where(c => c.Commentaries?.Any() == true || c.TimelineType.IsHighlightEvent())
                        .OrderByDescending(t => t.MatchTime)
                        .ThenByDescending(t => t.Time)
                        .ToList();

                if (matchCommentaries.Count > 0)
                {
                    FullMatchCommentaries = matchCommentaries
                        .Select(c => new CommentaryItemViewModel(c, DependencyResolver));
                    DefaultMatchCommentaries = FullMatchCommentaries.Take(DefaultLoadingCommentaryItemCount);

                    MatchCommentaries = new ObservableCollection<CommentaryItemViewModel>(DefaultMatchCommentaries);

                    HasCommentariesData = true;
                    VisibleShowMore = FullMatchCommentaries.Count() > DefaultLoadingCommentaryItemCount;
                }
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