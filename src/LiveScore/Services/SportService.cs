namespace LiveScore.Services
{
    using System.Collections.Generic;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Constants;
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
               new SportItem { Id = (int)SportType.Soccer, Name = SportType.Soccer.GetDescription() },
               new SportItem { Id = (int)SportType.Basketball, Name = SportType.Basketball.GetDescription() }
           };
        }
    }
}