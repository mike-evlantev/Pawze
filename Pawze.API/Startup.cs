using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Pawze.API.Infrastructure;
using Pawze.Core.Domain;
using Pawze.Core.Infrastructure;
using Pawze.Core.Repository;
using Pawze.Core.Services.Finance;
using Pawze.Data.Infrastructure;
using Pawze.Data.Repository;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;
using SimpleInjector.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

[assembly: OwinStartup(typeof(Pawze.API.Startup))]

namespace Pawze.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = ConfigureSimpleInjector(app);

            HttpConfiguration config = new HttpConfiguration
            {
                DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container)
            };
            WebApiConfig.Register(config);

            ConfigureOAuth(app, container);

            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app, Container container)
        {
            Func<IAuthorizationRepository> authRepositoryFactory = container.GetInstance<IAuthorizationRepository>;

            var authenticationOptions = new OAuthBearerAuthenticationOptions();
           

            var authorizationOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new PawzeAuthorizationServerProvider(authRepositoryFactory)
            };

            app.UseOAuthAuthorizationServer(authorizationOptions);
            app.UseOAuthBearerAuthentication(authenticationOptions);
        }

        public Container ConfigureSimpleInjector(IAppBuilder app)
        {
            var container = new Container();

            container.Options.DefaultScopedLifestyle = new ExecutionContextScopeLifestyle();

            // Infrastructure
            container.Register<IDatabaseFactory, DatabaseFactory>(Lifestyle.Scoped);
            container.Register<IUnitOfWork, UnitOfWork>();
            container.Register<IUserStore<PawzeUser>, UserStore>(Lifestyle.Scoped);

            // Repositories
            container.Register<IBoxItemRepository, BoxItemRepository>();
            container.Register<IBoxRepository, BoxRepository>();
            container.Register<IInventoryRepository, InventoryRepository>();
            container.Register<IPawzeConfigurationRepository, PawzeConfigurationRepository>();
            container.Register<IPawzeUserRepository, PawzeUserRepository>();
            container.Register<IRoleRepository, RoleRepository>();
            container.Register<IShipmentRepository, ShipmentRepository>();
            container.Register<ISubscriptionRepository, SubscriptionRepository>();
            container.Register<IUserRoleRepository, UserRoleRepository>();
            container.Register<IAuthorizationRepository, AuthorizationRepository>(Lifestyle.Scoped);

            // Services
            container.Register<ISubscriptionService, StripeSubscriptionService>();

            app.Use(async (context, next) =>
            {
                using (container.BeginExecutionContextScope())
                {
                    await next();
                }
            });

            container.Verify();

            return container;
        }
    }
}