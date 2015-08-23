using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(MeetyChat.Services.Startup))]

namespace MeetyChat.Services
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
