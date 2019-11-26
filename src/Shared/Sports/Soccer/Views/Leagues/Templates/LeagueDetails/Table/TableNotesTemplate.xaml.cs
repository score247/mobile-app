using System.Collections.Generic;
using LiveScore.Core.Models.Leagues;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Soccer.Views.Leagues.Templates.LeagueDetails.Table
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TableNotesTemplate : ContentView
    {
        public TableNotesTemplate()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty NotesProperty = BindableProperty.Create(
            nameof(Notes),
            typeof(IEnumerable<LeagueGroupNote>),
            typeof(TableNotesTemplate),
            propertyChanged: OnNotesPropertyChange);

        public IEnumerable<LeagueGroupNote> Notes
        {
            get => (IEnumerable<LeagueGroupNote>)GetValue(NotesProperty);
            set => SetValue(NotesProperty, value);
        }

        private static void OnNotesPropertyChange(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is TableNotesTemplate control) || newValue == null || newValue == oldValue)
            {
                return;
            }

            var notes = newValue as IEnumerable<LeagueGroupNote>;
            control.Content = BuildNotes(control, notes);
        }

        private static StackLayout BuildNotes(TableNotesTemplate control, IEnumerable<LeagueGroupNote> notes)
        {
            var content = new StackLayout();

            foreach (var note in notes)
            {
                var teamNameLabel = new Label
                {
                    Text = note.TeamName + ": ",
                    Style = (Style)control.Resources["MatchDetailTableNotes"]
                };

                var commentsText = string.Join("\r\n", note.Comments);
                var commentsLabel = new Label
                {
                    Text = commentsText,
                    Style = (Style)control.Resources["MatchDetailTableNotes"]
                };

                var noteLayout = new FlexLayout();
                noteLayout.Children.Add(teamNameLabel);
                noteLayout.Children.Add(commentsLabel);

                content.Children.Add(noteLayout);
            }

            return content;
        }
    }
}