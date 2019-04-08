﻿namespace LiveScore.Domain.Models.Teams
{
    public interface ITeamStatistic
    {
        int Possession { get; }

        int Fouls { get; }

        int Injuries { get; }
    }
}