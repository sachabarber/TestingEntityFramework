using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autofac;
using Autofac.Core.Activators;

using EFTest.Entities;

namespace EFTest
{
    public class IOCManager
    {
        private static IOCManager instance;

        static IOCManager()
        {
            instance = new IOCManager();
        }

        private IOCManager()
        {
            var builder = new ContainerBuilder();
 
            // Register individual components
            builder.RegisterType<SachaContext>()
                .As<ISachaContext>()
                .WithParameter("nameOrConnectionString", "SachaTestContextConnection")
                .InstancePerLifetimeScope();

            builder.RegisterType<SachaLazyContext>()
                .As<ISachaLazyContext>()
                .WithParameter("nameOrConnectionString", "SachaTestContextConnection")
                .InstancePerLifetimeScope();

            builder.RegisterType<SomeService>()
                .As<ISomeService>().InstancePerLifetimeScope();

            builder.RegisterType<SomeServiceLazy>()
                .As<ISomeServiceLazy>().InstancePerLifetimeScope();

         
 
            Container = builder.Build();
        }


        public IContainer Container { get; private set; }


        public static  IOCManager Instance
        {
            get
            {
                return instance;
            }
        }


    }
}
