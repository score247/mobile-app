namespace LiveScore.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiveScore.Domain.Models.Matches;
    using LiveScore.Features.Matches;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly MatchService matchService;

        public MatchController(MatchService matchService)
        {
            this.matchService = matchService;
        }

        [HttpGet("GetMatches")]
        public async Task<IEnumerable<Match>> GetMatches(int sportId, DateTime from, DateTime to, string language)
        {
            return await matchService.GetMatches(1, DateTime.Now, DateTime.Now.AddDays(1), language);
        }
    }
}