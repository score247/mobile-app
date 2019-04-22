namespace LiveScore.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Core.ViewModels;
    using LiveScore.Core.Constants;
    using LiveScore.Core.Events;
    using LiveScore.Core.Factories;
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
            if (SelectedSportItem.Id != 0)
            {
                var isSportChanged = (int)SettingsService.CurrentSport != SelectedSportItem.Id;
                SettingsService.CurrentSport = (SportType)Enum.Parse(typeof(SportType), SelectedSportItem.Name);

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