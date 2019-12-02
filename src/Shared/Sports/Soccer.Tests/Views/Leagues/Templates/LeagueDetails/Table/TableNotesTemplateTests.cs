using System.Collections.Generic;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.Soccer.Views.Leagues.Templates.LeagueDetails.Table;
using Xamarin.Forms;
using Xunit;

namespace Soccer.Tests.Views.Leagues.Templates.LeagueDetails.Table
{
    public class TableNotesTemplateTests : IClassFixture<ResourcesFixture>
    {
        [Fact]
        public void Notes_GetSetValueFromNotesProp()
        {
            // Arrange
            var template = new TableNotesTemplate();
            var notes = new List<LeagueGroupNote> { new LeagueGroupNote("1", "Name", new string[] { }) };

            // Act
            template.Notes = notes;

            // Assert
            var actualNotes = template.GetValue(TableNotesTemplate.NotesProperty);
            Assert.Equal(notes, actualNotes);
        }

        [Fact]
        public void OnNotesPropertyChange_NewValueIsNull_NotBuildContent()
        {
            // Arrange
            var template = new TableNotesTemplate();

            // Act
            TableNotesTemplate.OnNotesPropertyChange(template, null, null);

            // Assert
            Assert.Null(template.Content);
        }

        [Fact]
        public void OnNotesPropertyChange_NewValueHasData_BuildNotesContent()
        {
            // Arrange
            var template = new TableNotesTemplate();
            var notes = new List<LeagueGroupNote> {
                new LeagueGroupNote("1", "Fullham", new string[] { "Note1", "Note2" }),
                new LeagueGroupNote("2", "Chelsea", new string[] { "Note3", "Note4" })
            };

            // Act
            TableNotesTemplate.OnNotesPropertyChange(template, null, notes);

            // Assert
            var note1Layout = (template.Content as StackLayout).Children[0] as FlexLayout;
            var note1TeamLabel = note1Layout.Children[0] as Label;
            var note1CommentLabel = note1Layout.Children[1] as Label;
            Assert.Equal("Fullham: ", note1TeamLabel.Text);
            Assert.Equal(template.Resources["MatchDetailTableNotes"], note1TeamLabel.Style);
            Assert.Equal("Note1\r\nNote2", note1CommentLabel.Text);
            Assert.Equal(template.Resources["MatchDetailTableNotes"], note1CommentLabel.Style);

            var note2Layout = (template.Content as StackLayout).Children[1] as FlexLayout;
            var note2TeamLabel = note2Layout.Children[0] as Label;
            var note2CommentLabel = note2Layout.Children[1] as Label;
            Assert.Equal("Chelsea: ", note2TeamLabel.Text);
            Assert.Equal(template.Resources["MatchDetailTableNotes"], note2TeamLabel.Style);
            Assert.Equal("Note3\r\nNote4", note2CommentLabel.Text);
            Assert.Equal(template.Resources["MatchDetailTableNotes"], note2CommentLabel.Style);
        }
    }
}