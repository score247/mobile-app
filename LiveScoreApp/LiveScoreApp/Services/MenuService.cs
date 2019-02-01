namespace LiveScoreApp.Services
{
    using LiveScoreApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IMenuService
    {
        IList<MenuItem> GetAll();
    }

    public class MenuService : IMenuService
    {
        public IList<MenuItem> GetAll()
        {
            return new List<MenuItem>
            {
                new MenuItem { Title = "Soccer" },
                new MenuItem { Title = "Hockey" }
            };
        }
    }
}