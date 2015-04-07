using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using Ninject;
using ServiceStack.Configuration;
using Zeitgeist.Core.Data;
using Zeitgeist.Core.Data.Interfaces;
using Zeitgeist.Core.Patterns.Observer;
using Zeitgeist.Web.Data;

using Zeitgeist.Web.Models.Page;
using Zeitgeist.Web.Ninject;
using Zeitgeist.Web.Services;
using Zeitgeist.Web.Tools;

namespace Zeitgeist.Web.App_Start
{
    public class LocalNinjectDependencyResolver : System.Web.Http.Dependencies.IDependencyResolver
    {
        private readonly IKernel _kernel;

        public LocalNinjectDependencyResolver(IKernel kernel)
        {
            kernel.Load(Assembly.GetExecutingAssembly());
            kernel.Bind<IDbContext>().To<Context>().InSingletonScope();
            kernel.Bind(typeof(IRepository<>)).To(typeof(EfRepository<>));
            kernel.Bind<HttpContext>().ToMethod(ctx => HttpContext.Current).InTransientScope();
            kernel.Bind<HttpContextBase>().ToMethod(ctx => new HttpContextWrapper(HttpContext.Current)).InTransientScope();
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<Security>().To<Security>();
            kernel.Bind<PictureService>().To<PictureService>();
            kernel.Bind<InMemoryCache>().To<InMemoryCache>().InSingletonScope();
            kernel.Bind<Media>().To<Media>();
            kernel.Bind<RetoService>().To<RetoService>();
            kernel.Bind<IContainerAdapter>().To<NinjectIocAdapter>().WithConstructorArgument("kernel", kernel);
            kernel.Bind<ISubject<Notification>>().To<Subject<Notification>>().InSingletonScope();
            _kernel = kernel;
            
        }
        public System.Web.Http.Dependencies.IDependencyScope BeginScope()
        {
            return this;
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _kernel.GetAll(serviceType);
            }
            catch (Exception)
            {
                return new List<object>();
            }
        }

        public void Dispose()
        {
            // When BeginScope returns 'this', the Dispose method must be a no-op.
        }
    }
}