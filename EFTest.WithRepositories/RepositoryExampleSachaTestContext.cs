using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EFTest.Entities;

namespace EFTest.WithRepositories
{
    public class RepositoryExampleSachaTestContext : EfDataContextBase, ISachaTestContext
    {
        public RepositoryExampleSachaTestContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = false;
        }



        public DbSet<Post> Posts { get; set; }

        public void DoSomethingDirectlyWithDatabase()
        {
            //Not done for this example
        }
    }
}
