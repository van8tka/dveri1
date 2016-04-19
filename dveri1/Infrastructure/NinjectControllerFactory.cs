
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            kernel.Bind<IVhodnyeDvRepository>().To<EfVhodnyeDvRepository>();
            kernel.Bind<ISliderRepository>().To<EfSliderRepository>();
            kernel.Bind<IContactRepository>().To<EfContactRepository>();
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