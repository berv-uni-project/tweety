using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tweety.Models;

namespace Tweety.Controllers
{
    public class ResultController : Controller
    {
        public ActionResult ShowResult()
        {
            TweetResult rslt = (TweetResult)TempData["TweetAct"];
            return View(rslt);
        }

        public ActionResult NoResult()
        {
            return View();
        }



    }
}