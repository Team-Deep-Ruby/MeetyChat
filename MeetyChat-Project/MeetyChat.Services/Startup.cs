using MeetyChat.Services;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace MeetyChat.Services
{
    using System.Reflection;
    using System.Web.Http;
    using Data.Data;
    using Data.Interfaces;
    using Infrastructure;
    using Ninject;
    using Ninject.Web.Common.OwinHost;
    using Ninject.Web.WebApi.OwinHost;
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.UseNinjectMiddleware(CreateKernel).UseNinjectWebApi(GlobalConfiguration.Configuration);
        }

        private static StandardKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            RegisterMappings(kernel);
            return kernel;
        }

        private static void RegisterMappings(StandardKernel kernel)
        {
            kernel.Bind<IMeetyChatData>().To<MeetyChatData>()
                .WithConstructorArgument("data",
                    c => new MeetyChatDbContext());

            kernel.Bind<IUserIdProvider>().To<AspNetUserIdProvider>();
        }
    }
}
