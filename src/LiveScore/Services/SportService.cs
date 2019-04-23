namespace LiveScore.Services
{
    using System.Collections.Generic;
    using LiveScore.Core.Enumerations;
    using LiveScore.Models;

    public interface ISportService
    {
        IEnumerable<SportItem> GetSportItems();
    }

    public class SportService : ISportService
    {
        public IEnumerable<SportItem> GetSportItems()
        {
            return new List<SportItem>
           {
               new SportItem { Type = SportTypes.Soccer },
               new SportItem { Type = SportTypes.Basketball  }
           };
        }
    }
}