namespace LiveScore.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class EventCondition
    {
        public int Attendance { get; set; }

        public Venue Venue { get; set; }
    }

    ////"referee": {
    ////        "id": "sr:referee:52368",
    ////        "name": "Skomina, Damir",
    ////        "nationality": "Slovenia",
    ////        "country_code": "SVN"
    ////    },
    public class Referee
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
