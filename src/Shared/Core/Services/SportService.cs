namespace LiveScore.Core.Services
{
    using System.Collections.Generic;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models;

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
               new SportItem { Type = SportType.Soccer },
               new SportItem { Type = SportType.Basketball  }
           };
        }
    }
}