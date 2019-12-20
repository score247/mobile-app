using System.Collections.ObjectModel;
using System.Windows.Input;
using LiveScore.Common.Extensions;
using LiveScore.Core.ViewModels;
using Prism.Commands;

namespace LiveScore.Core.Controls.SearchPage
{
    public class SearchViewModel : ViewModelBase
    {
        public SearchViewModel()
        {
            TextChangeCommand = new DelegateCommand(OnTextChangeCommandExecuted);
        }

        public string SearchText { get; set; }

        public string PlaceholderText { get; internal set; }

        public ObservableCollection<SearchSuggestion> SuggestionItemSource { get; internal set; }

        public DelegateAsyncCommand CancelCommand { get; internal set; }

        public DelegateCommand TextChangeCommand { get; }

        public ICommand SearchCommand { get; internal set; }

        private void OnTextChangeCommandExecuted()
        {
            SearchCommand?.Execute(SearchText);
        }
    }
}