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
        }

        public HtmlWebViewSource WidgetContent { get; set; }

        public bool TrackerVisible { get; set; }

        public DelegateCommand OnCollapseTapped => new DelegateCommand(() => { TrackerVisible = !TrackerVisible; });

        public async override void OnAppearing()
        {
            base.OnAppearing();

            var formatMatchId = matchId.Replace("sr:match:", string.Empty);

            var content  = await File.ReadAllTextAsync(DependencyService.Get<IBaseUrl>().Get() + "/html/TrackerWidget.html");            

            WidgetContent = new HtmlWebViewSource();
            WidgetContent.Html = content.Replace("input-match-id", formatMatchId);       
        }
    }
}