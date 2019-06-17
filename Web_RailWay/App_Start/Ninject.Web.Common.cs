[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Web_RailWay.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Web_RailWay.App_Start.NinjectWebCommon), "Stop")]

namespace Web_RailWay.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using Ninject.Web.WebApi;
    using System.Web.Http;
    using Ninject.Web.Mvc;
    using System.Web.Mvc;
    using System.Web.Http.Dependencies;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        public class NinjectDependencyResolver : NinjectDependencyScope, System.Web.Http.Dependencies.IDependencyResolver, System.Web.Mvc.IDependencyResolver
        {
            private readonly IKernel kernel;

            public NinjectDependencyResolver(IKernel kernel)
                : base(kernel)
            {
                this.kernel = kernel;
            }

            public IDependencyScope BeginScope()
            {
                return new NinjectDependencyScope(this.kernel.BeginBlock());
            }
        }

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(kernel);
                var ninjectResolver = new NinjectDependencyResolver(kernel);
                
                DependencyResolver.SetResolver(ninjectResolver); // MVC 

                GlobalConfiguration.Configuration.DependencyResolver = ninjectResolver;


                //GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
                return kernel;

               

            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<EFMT.Abstract.IMT>().To<EFMT.Concrete.EFMetallurgTrans>();
            kernel.Bind<EFRC.Abstract.ISAP>().To<EFRC.Concrete.EFSAP>();
            kernel.Bind<EFKIS.Abstract.ITKIS>().To<EFKIS.Concrete.EFTKIS>();
            kernel.Bind<EFRW.Abstract.IRailWay>().To<EFRW.Concrete.EFRailWay>();
            kernel.Bind<EFTD.Abstract.IRepository<EFTD.Entities.MarriageWork>>().To<EFOC.Concrete.EFMarriageWork>();
            kernel.Bind<EFTD.Abstract.IRepository<EFTD.Entities.MarriagePlace>>().To<EFOC.Concrete.EFMarriagePlace>();
            kernel.Bind<EFTD.Abstract.IRepository<EFTD.Entities.MarriageClassification>>().To<EFOC.Concrete.EFMarriageClassification>();
            kernel.Bind<EFTD.Abstract.IRepository<EFTD.Entities.MarriageNature>>().To<EFOC.Concrete.EFMarriageNature>();
            kernel.Bind<EFTD.Abstract.IRepository<EFTD.Entities.MarriageCause>>().To<EFOC.Concrete.EFMarriageCause>();
            kernel.Bind<EFTD.Abstract.IRepository<EFTD.Entities.MarriageSubdivision>>().To<EFOC.Concrete.EFMarriageSubdivision>();

        }        
    }
}