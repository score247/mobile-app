using LiveScore.Common.LangResources;
using LiveScore.Core;
using Prism.Navigation;
using Xamarin.Essentials;

namespace LiveScore.Features.Menu.ViewModels
{
    public class AboutScore247ViewModel : MenuViewModelBase
    {
        public AboutScore247ViewModel(INavigationService navigationService, IDependencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
            VersionTracking.Track();
            SetupAppVersion();
        }
        public string AppVersion { get; private set; }
        private void SetupAppVersion()
        {
            AppVersion = string.Format(AppResources.Version, VersionTracking.CurrentVersion);
        }
    }
}