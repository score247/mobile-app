namespace LiveScore.Services
{
    using System.Collections.Generic;
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
               new SportItem { Id = 1, Name = "Soccer" }
           };
        }
    }
}