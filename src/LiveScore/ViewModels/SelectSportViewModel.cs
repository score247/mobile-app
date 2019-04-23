namespace LiveScore.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Core.ViewModels;
    using LiveScore.Core;
    using LiveScore.Models;
    using Prism.Events;
    using Prism.Navigation;

    public class SelectSportViewModel : ViewModelBase
    {
        public SelectSportViewModel(
            INavigationService navigationService,
            IDepdendencyResolver serviceLocator,
            IEventAggregator eventAggregator)
                : base(navigationService, serviceLocator, eventAggregator)
        {
            SelectSportItemCommand = new DelegateAsyncCommand(OnSelectSportItem);
            DoneCommand = new DelegateAsyncCommand(OnDone);
        }

        public SportItem SelectedSportItem { get; set; }

        public ObservableCollection<SportItem> SportItems { get; private set; }

        public DelegateAsyncCommand SelectSportItemCommand { get; }

        public DelegateAsyncCommand DoneCommand { get; }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            SportItems = new ObservableCollection<SportItem>(parameters["sportItems"] as IEnumerable<SportItem>);
        }

        private async Task OnSelectSportItem()
        {
            if (SelectedSportItem.Type != null)
            {
                var isSportChanged = SettingsService.CurrentSportType.Value != SelectedSportItem.Type.Value;
                SettingsService.CurrentSportType = SelectedSportItem.Type;

                if (isSportChanged)
                {
                    await NavigateToHome();
                }
            }
        }

        private async Task OnDone()
        {
            await NavigationService.GoBackAsync(useModalNavigation: true);
        }
    }
}