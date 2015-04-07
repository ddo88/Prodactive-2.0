using Microsoft.Owin;
using Owin;
[assembly: OwinStartup(typeof(Zeitgeist.Web.App_Start.Startup))]

namespace Zeitgeist.Web.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}
