using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InsuranceBackoffice.Startup))]
namespace InsuranceBackoffice
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
