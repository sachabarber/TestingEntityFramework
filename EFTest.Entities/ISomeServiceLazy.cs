using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using EFTest.Entities;

namespace EFTest
{
    public interface ISomeServiceLazy
    {
        //Sync
        void Insert();
        IEnumerable<PostLazy> GetAll();
        IEnumerable<PostLazy> GetAll(Expression<Func<PostLazy, bool>> filter);
        PostLazy FindById(int id);

        //Async
        Task InsertAsync();
        Task<IEnumerable<PostLazy>> GetAllAsync();
        Task<PostLazy> FindByIdAsync(int id);
    }
}
