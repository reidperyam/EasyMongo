﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Ninject.Modules;

namespace Ninject.Extensions.EasyMongo
{
    public class Configurator
    {
        private readonly IKernel _kernel;

        public Configurator()
        {
            var modules = new INinjectModule[] 
            {
                new EasyMongoExampleNinjectModule()
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