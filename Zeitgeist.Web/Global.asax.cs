using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Ninject;
using Ninject.Web.Common;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.Configuration;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.WebHost.Endpoints;
using Zeitgeist.Web.Api;
using Zeitgeist.Web.App_Start;
using Zeitgeist.Web.Data;


namespace Zeitgeist.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : NinjectHttpApplication
    {

        private IKernel kernel;
        public class ServicioAppHost : AppHostBase
        {
            private readonly IContainerAdapter _adapter;

            public ServicioAppHost(IContainerAdapter adapter) : base("Servicio Prueba", typeof (TestService).Assembly)
            {
                _adapter = adapter;
            }

            public override void Configure(Funq.Container container)
            {

                container.Adapter = _adapter;
                //DbRegistration
                //var connectionString = "";// ConfigurationManager.ConnectionStrings["Bd"].ConnectionString;
                //container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory(connectionString, PostgreSqlDialect.Provider));
                //IoC
                //IoC                
                //container.RegisterAutoWired<Negocio>();

                //Autentication
                Plugins.Add(new AuthFeature(() => new AuthUserSession(), new IAuthProvider[] { new BasicAuthProvider() }));
                container.Register<ICacheClient>(new MemoryCacheClient());

                var userRepository = new InMemoryAuthRepository();
                container.Register<IUserAuthRepository>(userRepository);
                string hash;
                string salt;
                var usuario = "A1b2c3d4"; //ConfigurationManager.AppSettings["user"].ToString();
                var pass = "A1b2c3d4";//ConfigurationManager.AppSettings["pass"].ToString();

                new SaltedHash().GetHashAndSaltString(pass, out hash, out salt);
                userRepository.CreateUserAuth(new UserAuth() { DisplayName = "Admin", Email = "admin@admin.com", FirstName = "pepe", LastName = "otro", UserName = usuario, PasswordHash = hash, Salt = salt }, pass);

                //Routes
                //    .Add<ServiceStatus>("/ServiceStatus")
                //    .Add<ServiceStatus>("/ServiceStatus/{A}");
            }
        }
        protected override void OnApplicationStarted()
         {
             Context c = new Context();
            
             if(c.Database.CreateIfNotExists())
                Database.SetInitializer<Context>(new SampleData(c));
            
             base.OnApplicationStarted();
             AreaRegistration.RegisterAllAreas();
             FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
             RouteConfig.RegisterRoutes(RouteTable.Routes);
             BundleConfig.RegisterBundles(BundleTable.Bundles);
            new ServicioAppHost(kernel.Get<IContainerAdapter>()).Init();
         }  

        protected override IKernel CreateKernel()
        {
                   kernel = new StandardKernel();
             //container.Adapter = new NinjectIocAdapter(kernel);
                    GlobalConfiguration.Configuration.DependencyResolver = new LocalNinjectDependencyResolver(kernel);
             //GlobalConfiguration.Configuration.DependencyResolver = new LocalNinjectDependencyResolver(kernel); 
             return kernel;
        }
    }

}