using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Nearsens.Web.Startup))]
namespace Nearsens.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
