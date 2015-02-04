using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autofac;
using Autofac.Core.Activators;

using EFTest.Entities;

namespace EFTest.WithRepositories
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
            builder.RegisterType<RepositoryExampleSachaTestContext>()
               .As<IUnitOfWork>()
               .WithParameter("nameOrConnectionString", "SachaTestContextConnection")
               .InstancePerLifetimeScope();

            builder.RegisterType<SomeService>()
               .As<ISomeService>().InstancePerLifetimeScope();


            builder.RegisterGeneric(typeof(Repository<>))
               .As(typeof(IRepository<>))
               .InstancePerLifetimeScope();



            Container = builder.Build();
        }


        public IContainer Container { get; private set; }


        public static IOCManager Instance
        {
            get
            {
                return instance;
            }
        }


    }
}
