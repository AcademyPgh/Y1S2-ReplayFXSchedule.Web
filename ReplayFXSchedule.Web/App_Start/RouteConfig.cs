using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ReplayFXSchedule.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "Schedule",
                url: "schedule/{category}",
                defaults: new { controller = "Public", action = "Index", category = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Games",
                url: "games/{gametype}",
                defaults: new { controller = "Public", action = "Games", gametype = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "ScheduleWeb",
                url: "daily/{date}/{category}",
                defaults: new { controller = "Public", action = "ScheduleWeb", date = "1-1-2017", category = UrlParameter.Optional }
                );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}
