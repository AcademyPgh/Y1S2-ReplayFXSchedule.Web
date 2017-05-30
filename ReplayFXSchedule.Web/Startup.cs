using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Web.Http.Cors;


[assembly: OwinStartup(typeof(ReplayFXSchedule.Web.Startup))]

namespace ReplayFXSchedule.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            ConfigureAuth(app);
        }
    }
}
