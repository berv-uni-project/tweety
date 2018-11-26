using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Tweety.Models;
using TweetyCore.Models;
using TweetyCore.Utils;

namespace TweetyCore.Controllers
{
    public class HomeController : Controller
    { 

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
                TwitterConnect twitterConnect = new TwitterConnect();
                TweetResponse result = twitterConnect.ProcessTag(tags);

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

