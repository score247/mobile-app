using System;
using System.Collections.Generic;
using System.Text;

namespace Tournament.Models
{
    public class Tournament
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public IList<Match> Matches { get; set; }
    }
}