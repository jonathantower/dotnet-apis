using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(identity.Startup))]
namespace identity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
