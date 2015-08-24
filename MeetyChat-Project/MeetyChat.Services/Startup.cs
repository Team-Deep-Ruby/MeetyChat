using MeetyChat.Services;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace MeetyChat.Services
{
    using System.Reflection;
    using System.Web.Http;
    using Ninject;
    using Ninject.Web.Common.OwinHost;
    using Ninject.Web.WebApi.OwinHost;
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            return;
            app.UseNinjectMiddleware(CreateKernel).UseNinjectWebApi(GlobalConfiguration.Configuration);
        }

        private static StandardKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            // RegisterMappings(kernel);
            return kernel;
        }

        private static void RegisterMappings(StandardKernel kernel)
        {
            // kernel.Bind<IMeetyChatData>().To<MeetyChatData>()
            //     .WithConstructorArgument("context",
            //         c => new MeetyChatDbContext());
            // 
            // kernel.Bind<IGameResultValidator>().To<GameResultValidator>();
            // 
            // kernel.Bind<IUserIdProvider>().To<AspNetUserIdProvider>();
        }
    }
}
