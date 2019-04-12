namespace LiveScore.Core.Models.Sports
{
    using System.Collections.Generic;
    using LiveScore.Core.Models.Leagues;

    public interface ISport : IEntity<string, string>
    {
        IEnumerable<ILeague> Leagues { get; set; }
    }
}