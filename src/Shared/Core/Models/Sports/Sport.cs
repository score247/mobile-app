namespace LiveScore.Core.Models.Sports
{
    using System.Collections.Generic;
    using Leagues;

    public interface ISport : IEntity<string, string>
    {
        IEnumerable<ILeague> Leagues { get; set; }
    }
}