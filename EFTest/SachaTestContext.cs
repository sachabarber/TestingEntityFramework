using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EFTest.Entities;

namespace EFTest
{

    public class SachaTestContext : DbContext, ISachaTestContext
    {
        public SachaTestContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }



        public DbSet<Post> Posts { get; set; }


        /// <summary>
        /// Call the database directly, without the interface this would have been a nightmare
        /// </summary>
        public void DoSomethingDirectlyWithDatabase()
        {

            var sql = @"INSERT INTO [SachaTest].[dbo].[Posts]
                               ([Url]
                               ,[Discriminator])
                         VALUES
                               ({0}
                               ,{1})";

            this.Database.ExecuteSqlCommand(sql, "SomeDirectUrl", "Post");
        }
    }
}
