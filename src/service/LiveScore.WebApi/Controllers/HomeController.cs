namespace LiveScore.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        public IActionResult Index()
            => new ContentResult
            {
                Content = "Live Scores API has been started."
            };
    }
}
