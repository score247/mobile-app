using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.ViewModels;
using PanCardView.EventArgs;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;

namespace LiveScore.Features.Favorites.ViewModels
{
    public class FavoriteViewModel : ViewModelBase
    {
        public FavoriteViewModel(
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            IEventAggregator eventAggregator)
            : base(navigationService, serviceLocator)
        {
            ItemAppearedCommand = new DelegateCommand<ItemAppearedEventArgs>(OnItemAppeared);
            ItemDisappearingCommand = new DelegateCommand<ItemDisappearingEventArgs>(OnItemDisappearing);

            FavoriteItemSources = new List<TabItemViewModel> {
                new FavoriteMatchesViewModel(NavigationService, DependencyResolver, eventAggregator),
                new FavoriteTeamsViewModel(NavigationService, DependencyResolver, eventAggregator),
                new FavoriteLeaguesViewModel(NavigationService, DependencyResolver, eventAggregator)
            };
        }

        public IReadOnlyList<TabItemViewModel> FavoriteItemSources { get; }

        public byte SelectedIndex { get; set; }

        public TabItemViewModel SelectedItem { get; set; }

        public DelegateCommand<ItemAppearedEventArgs> ItemAppearedCommand { get; }

        public DelegateCommand<ItemDisappearingEventArgs> ItemDisappearingCommand { get; }

        public override async Task OnNetworkReconnectedAsync()
        {
            await base.OnNetworkReconnectedAsync();

            await SelectedItem?.OnNetworkReconnectedAsync();
        }

        public override void OnResumeWhenNetworkOK()
        {
            base.OnResumeWhenNetworkOK();

            SelectedItem?.OnAppearing();
        }

        public override void OnSleep()
        {
            base.OnSleep();

            SelectedItem?.OnDisappearing();
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();

            SelectedItem?.OnDisappearing();
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            if (!IsActive)
            {
                return;
            }

            if (SelectedItem != null)
            {
                SelectedItem.IsActive = true;
                SelectedItem.OnAppearing();
            }
            else
            {
                var firstItem = FavoriteItemSources[0];
                firstItem.IsActive = true;
                firstItem.OnAppearing();
            }
        }

        private void OnItemAppeared(ItemAppearedEventArgs args)
        {
            if (!IsActive)
            {
                return;
            }

            var selectedItem = FavoriteItemSources[args.Index];

            if (selectedItem != null)
            {
                selectedItem.IsActive = true;
                selectedItem.OnAppearing();
            }
        }

        private void OnItemDisappearing(ItemDisappearingEventArgs args)
        {
            if (args.Index >= 0)
            {
                var previousItem = FavoriteItemSources[args.Index];

                if (previousItem == null)
                {
                    return;
                }

                previousItem.IsActive = false;
                previousItem.OnDisappearing();
            }
        }
    }
}