namespace LiveScore.Services
{
    using System.Collections.Generic;
    using LiveScore.Core.Constants;
    using LiveScore.Models;

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
                new MenuItem { Id = (int)SportType.Soccer, Title = "Soccer", IconValue = "\ue902", GroupId = 1, GroupName = "Sports" },
                new MenuItem { Id = (int)SportType.BasketBall, Title = "BasketBall", IconValue = "\ue900", GroupId = 1, GroupName = "Sports" },
                new MenuItem { Id = (int)SportType.ESports, Title = "E-Sports", IconValue = "\ue901", GroupId = 1, GroupName = "Sports" }
            };
        }
    }
}