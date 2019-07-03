namespace LiveScore.Core.Models.Odds
{
    public interface IBookmaker: IEntity<string, string>
    {
    }

    public class Bookmaker : Entity<string, string>, IBookmaker
    {

    }
}
