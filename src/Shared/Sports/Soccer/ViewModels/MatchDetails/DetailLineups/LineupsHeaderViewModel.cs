using LiveScore.Common.Services;
using Xamarin.Forms;

namespace LiveScore.Soccer.ViewModels.MatchDetails.DetailLineups
{
    public class LineupsHeaderViewModel
    {
#pragma warning disable S103 // Lines should not be too long
        private const string pitchViewHtml = "<html><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><body style=\"padding: 0; margin: 0;\">{0}</body></html>";
        private const string formationSplit = "-";
        private const int stadiumWidth = 357;
        private const int stadiumHeight = 530;
        private const string formationFormattedSplit = " - ";

        public LineupsHeaderViewModel(
            string pitchView,
            IDeviceInfo deviceInfo,
            string homeName,
            string homeFormation,
            string awayName,
            string awayFormation)
        {
            LineupsPitchView = new HtmlWebViewSource { Html = string.Format(pitchViewHtml, pitchView) };
            PitchViewHeight = (int)(deviceInfo.Width  / deviceInfo.Density / stadiumWidth * stadiumHeight);
            HomeName = homeName?.ToUpperInvariant();
            HomeFormation = homeFormation?.Replace(formationSplit, formationFormattedSplit);
            AwayName = awayName?.ToUpperInvariant();
            AwayFormation = awayFormation?.Replace(formationSplit, formationFormattedSplit);
        }

#pragma warning restore S103 // Lines should not be too long

        public HtmlWebViewSource LineupsPitchView { get; }

        public int PitchViewHeight { get; }

        public string HomeName { get; }

        public string HomeFormation { get; }

        public string AwayName { get; }

        public string AwayFormation { get; }
    }
}