using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ClaimsProjectFinal.Startup))]
namespace ClaimsProjectFinal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
