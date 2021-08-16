using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using TweetyCore.Models;
using TweetyCore.Utils.Twitter;

namespace TweetyCore.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        [AllowAnonymous]
        [HttpGet("/")]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet("About")]
        public IActionResult About()
        {
            return View();
        }

        [HttpPost("/")]
        public async Task<IActionResult> Index([FromServices] ITwitterConnect twitter, Tags tags)
        {
            if (ModelState.IsValid)
            {
                var result = await twitter.ProcessTag(tags);
                if (result.Count > 0)
                {
                    return View("ShowResult", result.Data);
                }
                else
                {
                    return View("NoResult");
                };
            }
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

