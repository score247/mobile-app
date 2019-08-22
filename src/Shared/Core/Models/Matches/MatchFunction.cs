namespace LiveScore.Core.Models.Matches
{
    public class MatchFunction
    {
        public MatchFunction(string abbreviation, string name)
        {
            Abbreviation = abbreviation;
            Name = name;
        }

        public string Abbreviation { get; }

        public string Name { get; }
    }
}
