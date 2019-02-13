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
                new MenuItem { Id = 1, Title = "Soccer", IconValue = "\ue902", GroupId = 1, GroupName = "Sports" },
                new MenuItem { Id = 2, Title = "BasketBall", IconValue = "\ue900", GroupId = 1, GroupName = "Sports" },
                new MenuItem { Id = 3, Title = "E-Sports", IconValue="\ue901", GroupId = 1, GroupName = "Sports" }
            };
        }
    }
}