namespace LiveScore.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiveScore.Domain.Models.Leagues;
    using LiveScore.Features.Leagues;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class LeagueController : ControllerBase
    {
        private readonly LeagueService leagueService;

        public LeagueController(LeagueService leagueService)
        {
            this.leagueService = leagueService;
        }

        [HttpGet("GetLeaguesByDate")]
        public async Task<IEnumerable<League>> GetLeaguesByDate(int sportId, DateTime from, DateTime to)
        {
            return await leagueService.GetLeagues(1, DateTime.Now, DateTime.Now.AddDays(1));
        }

        [HttpGet("GetLeagues")]
        public async Task<IEnumerable<League>> GetLeagues(int sportId)
        {
            return await leagueService.GetLeagues(1, DateTime.Now, DateTime.Now.AddDays(1));
        }
    }
}