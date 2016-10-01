
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using Domain2.Interfaces;
using Domain2.Implementations;


namespace dveri1.Infrastructure
{
    public class NinjectControllerFactory : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectControllerFactory(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        private void AddBindings()
        {
            kernel.Bind<IYstanovkaRepository>().To<EfYstanovkaRepository>();
            kernel.Bind<ICommentRepository>().To<EfCommentRepository>();
            kernel.Bind<IVhodnyeDvRepository>().To<EfVhodnyeDvRepository>();
            kernel.Bind<ISliderRepository>().To<EfSliderRepository>();
            kernel.Bind<IContactRepository>().To<EfContactRepository>();
            kernel.Bind<IKlientRepository>().To<EfKlientRepository>();
            kernel.Bind<IOplataDostavkaRepository>().To<EfOplataDostavkaRepository>();
            kernel.Bind<IUserRepository>().To<EfUserRepository>();
            kernel.Bind<ISeoMainRepository>().To<EfSeoMainRepository>();
            kernel.Bind<IArticlesRepository>().To<EfArticlesRepository>();
            kernel.Bind<IMegkomnatnyeDvRepository>().To<EfMegkomnatnyeDvRepository>();
            kernel.Bind<IColorsRepository>().To<EfColorsRepository>();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
    }
}