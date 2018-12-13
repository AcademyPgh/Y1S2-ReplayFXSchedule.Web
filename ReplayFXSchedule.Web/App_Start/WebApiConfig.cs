using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Net.Http;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ReplayFXSchedule.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.Formatters.Add(new BrowserJsonFormatter());
            // Web API routes

            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "ScheduleV2",
            //    routeTemplate: "api/v2/schedule/{convention_id}/{category}",
            //    defaults: new { controller = "APIV2", action = "Index", convention_id = RouteParameter.Optional, category = RouteParameter.Optional }
            //);
            //config.Routes.MapHttpRoute(
            //    name: "GamesV2",
            //    routeTemplate: "api/v2/games/{convention_id}/{gametype}",
            //    defaults: new { controller = "APIV2", action = "Games", convention_id = RouteParameter.Optional, gametype = RouteParameter.Optional }
            //    );

            //config.Routes.MapHttpRoute(
            //    name: "ScheduleWebV2",
            //    routeTemplate: "api/v2/daily/{convention_id}/{date}/{category}",
            //    defaults: new { controller = "APIV2", action = "ScheduleWeb", date = "1-1-2017", convention_id = RouteParameter.Optional, category = RouteParameter.Optional }
            //    );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }


    public class BrowserJsonFormatter : JsonMediaTypeFormatter
    {
        public BrowserJsonFormatter()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            this.SerializerSettings.Formatting = Formatting.Indented;
            this.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            this.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, mediaType);
            headers.ContentType = new MediaTypeHeaderValue("application/json");
        }
    }
}
