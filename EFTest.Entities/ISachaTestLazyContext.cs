using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFTest.Entities
{
    public interface ISachaTestLazyContext : IDisposable
    {
        DbSet<PostLazy> Posts { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync();

        //hide the call to DbCOntext.Database behind an interface so it to, can be mocked
        void DoSomethingDirectlyWithDatabase();

    }
}
