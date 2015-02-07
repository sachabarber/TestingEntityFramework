using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFTest.Entities;

namespace EFTest.Tests
{
    public class SachaContextTestDouble : DbContext, ISachaContext
    {
        public virtual DbSet<Post> Posts { get; set; }
        public void DoSomethingDirectlyWithDatabase()
        {
           
        }
    }
}
