namespace LiveScore.Features.Scores
{
    using System;
    using LiveScore.Domain.Models;

    public interface ScoreService
    {
        ScoresModel GetScore(int sportId, DateTime from, DateTime to);
    }

    public class ScoreServiceImpl : ScoreService
    {
        public ScoresModel GetScore(int sportId, DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }
    }
}
