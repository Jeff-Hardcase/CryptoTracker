using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ccDash.Startup))]
namespace ccDash
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
