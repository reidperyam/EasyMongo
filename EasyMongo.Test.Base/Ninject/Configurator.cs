using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Ninject.Injection;
using Ninject.Activation;
using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Modules;
using Ninject.Parameters;
using Ninject.Planning;
using Ninject.Selection;
using Ninject.Syntax;

namespace EasyMongo.Test.Base.Ninject
{
    public class Configurator
    {
        private readonly IKernel _kernel;

        public Configurator()
        {
            var modules = new INinjectModule[] 
            {
                new IntegrationTestNinjectModule()
            };

            _kernel = new StandardKernel(modules);
        }

        public IKernel Kernel
        {
            get { return _kernel; }
        }

        public T TryGet<T>()
        {
            return _kernel.TryGet<T>();
        }
    }
}
