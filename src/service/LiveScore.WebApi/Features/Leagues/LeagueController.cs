namespace LiveScore.WebApi.Features.Leagues
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiveScore.Features.Leagues.Models;
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
        public async Task<IEnumerable<League>> GetLeaguesByDate(int sportId, DateTime from, DateTime to, string language)
        {
            return await leagueService.GetLeagues(1, DateTime.Now, DateTime.Now.AddDays(1));
        }

        [HttpGet("GetLeagues")]
        public async Task<IEnumerable<League>> GetLeagues(int sportId, string language)
        {
            return await leagueService.GetLeagues(1, DateTime.Now, DateTime.Now.AddDays(1));
        }
    }
}