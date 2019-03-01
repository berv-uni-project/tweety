using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Tweety.Models;
using TweetyCore.Models;
using TweetyCore.Utils;
using TweetyCore.Utils.Twitter;

namespace TweetyCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITwitterConnect _twitter;
        public HomeController(ITwitterConnect twitter)
        {
            _twitter = twitter;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Tags tags)
        {
            if (ModelState.IsValid)
            {
                var result = _twitter.ProcessTag(tags);
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

