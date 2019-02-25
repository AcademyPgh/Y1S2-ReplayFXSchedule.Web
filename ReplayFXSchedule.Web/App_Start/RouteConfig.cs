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
                name: "CreateConvention",
                url: "Conventions/Create",
                defaults: new { controller = "Conventions", action = "Create" }
                );

            routes.MapRoute(
                name: "Logout",
                url: "Account/Logout",
                defaults: new { controller = "Account", action = "Logout" }
                );

            routes.MapRoute(
                name: "Login",
                url: "Account/Login",
                defaults: new { controller = "Account", action = "Login" }
                );

            routes.MapRoute(
                name: "Users",
                url: "Users/{action}/{id}",
                defaults: new { controller = "Users", action = "Index", id = UrlParameter.Optional}
                );

            routes.MapRoute(
                name: "ConventionDefault",
                url: "{controller}/{convention_id}/{action}/{id}",
                defaults: new { controller = "Conventions", action = "Index", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Conventions", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
