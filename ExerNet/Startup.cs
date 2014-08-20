using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Exernet.Startup))]
namespace Exernet
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
