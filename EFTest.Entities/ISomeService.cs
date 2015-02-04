using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using EFTest.Entities;

namespace EFTest
{
    public interface ISomeService
    {
        //Sync
        void Insert();
        IEnumerable<Post> GetAll();
        IEnumerable<Post> GetAll(Expression<Func<Post, bool>> filter);
        Post FindById(int id);

        //Async
        Task InsertAsync();
        Task<IEnumerable<Post>> GetAllAsync();
        Task<Post> FindByIdAsync(int id);
    }
}
