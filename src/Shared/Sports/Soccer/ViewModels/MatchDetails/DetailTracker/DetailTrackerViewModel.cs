namespace LiveScore.Soccer.ViewModels.DetailTracker
{
    using System.IO;
    using System.Threading.Tasks;
    using LiveScore.Common.PlatformDependency;
    using LiveScore.Core;
    using LiveScore.Core.Controls.TabStrip;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Soccer.Models.Matches;
    using Prism.Commands;
    using Prism.Navigation;
    using Xamarin.Forms;

    internal class DetailTrackerViewModel : TabItemViewModel
    {
        private const string RemoveMatchPrefix = "sr:match:";
        private const string ReplacePrefix = "input-match-id";
        private const string WidgetPrefix = "widget-url";
        private const string LanguagePrefix = "input-language";
        
        private const string LanguageCode = "en";

        private readonly MatchCoverage matchCoverage;

        public DetailTrackerViewModel(           
            MatchCoverage coverage,
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            DataTemplate dataTemplate)
            : base(navigationService, serviceLocator, dataTemplate)
        {
            matchCoverage = coverage;
            TrackerVisible = true;
            TrackerHidden = false;
        }

        public HtmlWebViewSource WidgetContent { get; set; }

        public bool TrackerVisible { get; set; }

        public bool TrackerHidden { get; set; }

        public bool HasTrackerData { get; set; }
        
        public DelegateCommand OnCollapseTracker => new DelegateCommand(CollapseTracker);        

        public DelegateCommand OnExpandTracker => new DelegateCommand(ExpandTracker);

        public override Task OnNetworkReconnected()
        {
            OnAppearing();

            return Task.CompletedTask;
        }

        public async override void OnAppearing()
        {
            base.OnAppearing();

            if (matchCoverage != null && matchCoverage.Coverage != null && matchCoverage.Coverage.Live)
            {
                WidgetContent = new HtmlWebViewSource
                {
                    Html = await GenerateTrackerWidget(matchCoverage.Coverage)
                };

                HasTrackerData = true;
                HasData = true;
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

        private void CollapseTracker()
        {
            TrackerVisible = false;
            TrackerHidden = true;
        }

        private void ExpandTracker()
        {
            TrackerVisible = true;
            TrackerHidden = false;
        }
    }
}