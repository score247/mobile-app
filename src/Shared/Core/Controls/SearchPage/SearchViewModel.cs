namespace LiveScore.Core.Controls.SearchPage
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.ViewModels;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Navigation;

    public class SearchViewModel : ViewModelBase
    {
        private static readonly List<string> Suggestions = new List<string>
        {
            "Chelsea",
            "Champion League",
            "Asernal",
            "Livepool",
            "Ajax",
            "Real Madrid",
            "Manchester United",
            "Barcelona",
            "Juventus",
            "Laliga",
            "Premiere League",
            "Serie A",
            "Bundesliga",
            "Aston Villa",
            "Manchester City",
            "Lazio",
            "Bayern Munchen"
        };

        public SearchViewModel(INavigationService navigationService, IDependencyResolver serviceLocator, IEventAggregator eventAggregator)
             : base(navigationService, serviceLocator, eventAggregator)
        {
        }

        public string SearchText { get; set; }

        public ObservableCollection<SearchSuggestion> SuggestionItemSource { get; set; }

        public DelegateAsyncCommand CancelCommand => new DelegateAsyncCommand(OnCancelCommandExecuted);

        private async Task OnCancelCommandExecuted()
        {
            await NavigationService.GoBackAsync(useModalNavigation: true);
        }

        public DelegateCommand TextChangeCommand => new DelegateCommand(OnTextChangeCommandExecuted);

        private void OnTextChangeCommandExecuted()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                SuggestionItemSource.Clear();
                return;
            }

            var searchSuggestions = Suggestions
                    .Where(s => s.Contains(SearchText.ToLowerInvariant()))
                    .Select(s => new SearchSuggestion { Name = s });

            SuggestionItemSource = new ObservableCollection<SearchSuggestion>(searchSuggestions);
        }
    }
}
