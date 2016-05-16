using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PropertyGuide.Startup))]
namespace PropertyGuide
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
