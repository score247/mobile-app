namespace LiveScore.Score.Models
{
    using LiveScore.Core.Models.Matches;
    using PropertyChanged;

    [AddINotifyPropertyChangedInterface]
    public class MatchData
    {
        public IMatch Match { get; set; }
    }
}
