using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Domain.Models.Leagues;
using LiveScore.Features.Leagues;
using Microsoft.AspNetCore.Mvc;

namespace LiveScore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeagueController : ControllerBase
    {
        private readonly LeagueService leagueService;

        public LeagueController(LeagueService leagueService)
        {
            this.leagueService = leagueService;
        }

        [HttpGet]
        public async Task<IEnumerable<League>> GetLeagues(int sportId, DateTime from, DateTime to)
        {
            return await leagueService.GetLeagues(1, DateTime.Now, DateTime.Now.AddDays(2));
        }
    }
}
