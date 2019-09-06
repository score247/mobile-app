namespace LiveScore.Core.Controls.SearchPage
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using ViewModels;
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
            CancelCommand = new DelegateAsyncCommand(OnCancelCommandExecuted);
            TextChangeCommand = new DelegateCommand(OnTextChangeCommandExecuted);
        }

        public string SearchText { get; set; }

        public ObservableCollection<SearchSuggestion> SuggestionItemSource { get; set; }

        public DelegateAsyncCommand CancelCommand { get; }

        public DelegateCommand TextChangeCommand { get; }

        private async Task OnCancelCommandExecuted()
        {
            await NavigationService.GoBackAsync(useModalNavigation: true);
        }

        private void OnTextChangeCommandExecuted()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                SuggestionItemSource?.Clear();
                return;
            }

            var searchSuggestions = Suggestions
                    .Where(s => s.IndexOf(SearchText, System.StringComparison.InvariantCultureIgnoreCase) >= 0)
                    .Select(s => new SearchSuggestion { Name = s })
                    .OrderBy(s => s.Name);

            SuggestionItemSource = new ObservableCollection<SearchSuggestion>(searchSuggestions);
        }
    }
}