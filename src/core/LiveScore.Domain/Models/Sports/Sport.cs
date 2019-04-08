﻿namespace LiveScore.Domain.Models.Sports
{
    using System.Collections.Generic;
    using LiveScore.Domain.Models.Leagues;

    public interface ISport : IEntity<string, string>
    {
        IEnumerable<League> Leagues { get; set; }
    }

    public class Sport : Entity<string, string>, ISport
    {
        public IEnumerable<League> Leagues { get; set; }
    }
}