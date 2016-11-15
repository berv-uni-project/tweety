using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Tweety
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "TweetyForm", action = "SearchForm", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "Result",
                "",
                new { controller = "Result", action = "ShowResult" }
            );

        }
    }
}
