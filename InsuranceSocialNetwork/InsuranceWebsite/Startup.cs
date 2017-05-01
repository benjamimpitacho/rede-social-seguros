using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InsuranceWebsite.Startup))]
namespace InsuranceWebsite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
