using LiveScoreApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveScoreApp.Services
{
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
               new SportItem {Id = 1, Name = "Soccer", NumberOfEvent = 2, IsVisible = true},
               new SportItem {Id = 2, Name = "Tennis", NumberOfEvent = 3},
               new SportItem {Id = 3, Name = "ESport", NumberOfEvent = 4},
               new SportItem {Id = 4, Name = "Hockey", NumberOfEvent = 5},
           };
        }
    }
}