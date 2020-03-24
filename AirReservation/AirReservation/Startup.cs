using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AirReservation.Startup))]
namespace AirReservation
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
