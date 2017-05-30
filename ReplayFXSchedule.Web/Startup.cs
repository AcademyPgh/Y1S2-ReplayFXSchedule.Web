using Microsoft.Owin;
using Owin;


[assembly: OwinStartupAttribute(typeof(ReplayFXSchedule.Web.Startup))]

namespace ReplayFXSchedule.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
