using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EFTest.WithRepositories
{
    public interface IRepository<T> where T : class
    {
        //sync
        int Count();
        void Add(T item);
        bool Contains(T item);
        bool Remove(T item);
        T Get(int id);
        T GetIncluding(int id, params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);


        //async
        Task<int> CountAsync();
        Task<bool> AddAsync(T item);
        Task<bool> ContainsAsync(T item);
        Task<bool> RemoveAsync(T item);
        Task<T> GetAsync(int id);
        Task<T> GetIncludingAsync(int id, params Expression<Func<T, object>>[] includeProperties);
        Task<IQueryable<T>> GetAllAsync();
        Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
        Task<IQueryable<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties);

    }
}
