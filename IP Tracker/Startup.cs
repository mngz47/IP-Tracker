using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: OwinStartup(typeof(IP_Tracker.Startup))]

namespace IP_Tracker
{
    public partial class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            //load geobase.dat into memory
            //Database.R_Buffer_Byte();
            //Database.R_Sequential_Byte();
            Database.RR_String();

        }
    }


}
