namespace LiveScore.Soccer.ViewModels.DetailTracker
{
    using System.IO;
    using System.Threading.Tasks;
    using LiveScore.Common.PlatformDependency;
    using LiveScore.Core;
    using LiveScore.Core.Controls.TabStrip;
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

        private const string Widget = "https://widgets.sir.sportradar.com/sportradar/widgetloader";


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

        public async override void OnAppearing()
        {
            base.OnAppearing();

            if (matchCoverage.Coverage != null && matchCoverage.Coverage.Live)
            {
                WidgetContent = new HtmlWebViewSource
                {
                    Html = await GenerateTrackerWidget()
                };

                HasTrackerData = true;
                HasData = true;
            }
            else
            {
                HasData = false;
            }
        }

        private async Task<string> GenerateTrackerWidget()
        {
            var formatMatchId = matchCoverage.MatchId.Replace(RemoveMatchPrefix, string.Empty);

            var content = await File.ReadAllTextAsync(DependencyService.Get<IBaseUrl>().Get() + "/html/TrackerWidget.html");

            content = content.Replace(ReplacePrefix, formatMatchId);
            content = content.Replace(WidgetPrefix, Widget);
            content = content.Replace(LanguagePrefix, "en");

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