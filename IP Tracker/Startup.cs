using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(IP_Tracker.Startup))]

namespace IP_Tracker
{
    public partial class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

        }
    }


}
