using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Core;
using LiveScore.Core.ViewModels;
using LiveScore.Soccer.Models.Teams;
using Prism.Navigation;

namespace LiveScore.Soccer.ViewModels.Teams
{
    public class SearchTeamsViewModel : ViewModelBase
    {
        public SearchTeamsViewModel(INavigationService navigationService, IDependencyResolver dependencyResolver)
            : base(navigationService, dependencyResolver)
        {
            CancelSearchCommand = new DelegateAsyncCommand(OnCancelSearch);
        }

        public DelegateAsyncCommand CancelSearchCommand { get; }

        public IReadOnlyCollection<TeamProfile> TeamsItemSource { get; private set; }

        public async Task OnCancelSearch()
        {
            await NavigationService.GoBackAsync();
        }
    }
}