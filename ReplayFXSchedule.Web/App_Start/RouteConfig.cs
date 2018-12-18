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
                url: "schedule/{convention_id}/{category}",
                defaults: new { controller = "Public", action = "Index", convention_id = UrlParameter.Optional, category = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "GamesList",
                url: "gameslist/{convention_id}/{gametype}",
                defaults: new { controller = "Public", action = "Games", convention_id = UrlParameter.Optional, gametype = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "ScheduleWeb",
                url: "daily/{convention_id}/{date}/{category}",
                defaults: new { controller = "Public", action = "ScheduleWeb", date = "1-1-2017", convention_id = UrlParameter.Optional, category = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}
