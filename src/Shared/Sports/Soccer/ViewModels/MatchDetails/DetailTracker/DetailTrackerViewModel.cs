namespace LiveScore.Soccer.ViewModels.DetailTracker
{
    using System.IO;
    using LiveScore.Common.PlatformDependency;
    using LiveScore.Core;
    using LiveScore.Core.Controls.TabStrip;
    using Prism.Commands;
    using Prism.Navigation;
    using Xamarin.Forms;

    internal class DetailTrackerViewModel : TabItemViewModel
    {
        private const string RemoveMatchPrefix = "sr:match:";
        private const string ReplacePrefix = "input-match-id";

        private readonly string matchId;

        public DetailTrackerViewModel(
            string matchId,
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            DataTemplate dataTemplate)
            : base(navigationService, serviceLocator, dataTemplate)
        {
            this.matchId = matchId;
            TrackerVisible = true;
            TrackerHidden = false;
        }

        public HtmlWebViewSource WidgetContent { get; set; }

        public bool TrackerVisible { get; set; }

        public bool TrackerHidden { get; set; }

        public DelegateCommand OnCollapseTracker => new DelegateCommand(CollapseTracker);        

        public DelegateCommand OnExpandTracker => new DelegateCommand(ExpandTracker);        

        public async override void OnAppearing()
        {
            base.OnAppearing();

            var formatMatchId = matchId.Replace(RemoveMatchPrefix, string.Empty);

            var content = await File.ReadAllTextAsync(DependencyService.Get<IBaseUrl>().Get() + "/html/TrackerWidget.html");

            WidgetContent = new HtmlWebViewSource
            {
                Html = content.Replace(ReplacePrefix, formatMatchId)
            };
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