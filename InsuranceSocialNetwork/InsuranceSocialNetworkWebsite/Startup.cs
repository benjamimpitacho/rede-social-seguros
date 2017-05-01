using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InsuranceSocialNetworkWebsite.Startup))]
namespace InsuranceSocialNetworkWebsite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
