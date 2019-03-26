using System;
namespace League.Models
{
    public class LeagueItem
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsGrouped { get; set; }

        public string GroupName { get; set; }
    }
}
