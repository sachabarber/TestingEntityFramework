using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using EFTest.Entities;

namespace EFTest.WithRepositories
{
    public class Repository<T> : IRepository<T> where T : class, IEntity 
    {
        private readonly IUnitOfWork context;

        public Repository(IUnitOfWork context)
        {
            this.context = context;
        }


        #region Sync
        public int Count()
        {
            return context.Get<T>().Count(); 
        }

        public void Add(T item)
        {
            context.Add(item);
        }

        public bool Contains(T item)
        {
            return context.Get<T>().FirstOrDefault(t => t == item) != null;
        }

        public bool Remove(T item)
        {
            return context.Remove(item);
        }

        public T Get(int id)
        {
            return context.Get<T>().SingleOrDefault(x => x.Id == id);
        }

        public T GetIncluding(int id, params Expression<Func<T, object>>[] includeProperties)
        {
            return GetAllIncluding(includeProperties).SingleOrDefault(x => x.Id == id);
        }


        public IQueryable<T> GetAll()
        {
            return context.Get<T>();
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return context.Get<T>().Where(predicate).AsQueryable<T>();
        }

        /// <summary>
        /// Used for Lazyloading navigation properties
        /// </summary>
        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> queryable = GetAll();
            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
            {
                queryable = queryable.Include(includeProperty);
            }
            return queryable;
        }

        #endregion

        #region Async
        public async Task<int> CountAsync()
        {
           return await Task.Run(() => context.Get<T>().Count()); 
        }

        public Task<bool> AddAsync(T item)
        {
            return Task.Run(() =>
                {
                    context.Add(item);
                    return true;
                });
        }

        public Task<bool> ContainsAsync(T item)
        {
            return Task.Run(() => context.Get<T>().FirstOrDefault(t => t == item) != null);
        }

        public Task<bool> RemoveAsync(T item)
        {
            return Task.Run(() => context.Remove(item));
            
        }

        public Task<T> GetAsync(int id)
        {
            return Task.Run(() => context.Get<T>().SingleOrDefault(x => x.Id == id));
        }

        public async Task<T> GetIncludingAsync(int id, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> queryable = await GetAllIncludingAsync(includeProperties);
            return await queryable.SingleOrDefaultAsync(x => x.Id == id);
        }

        public Task<IQueryable<T>> GetAllAsync()
        {
            return Task.Run(() => context.Get<T>());
        }

        public Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return Task.Run(() => context.Get<T>().Where(predicate).AsQueryable<T>());
        }

        /// <summary>
        /// Used for Lazyloading navigation properties
        /// </summary>
        public Task<IQueryable<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            return Task.Run(
                () =>
                {
                    IQueryable<T> queryable = GetAll();
                    foreach (Expression<Func<T, object>> includeProperty in includeProperties)
                    {
                        queryable = queryable.Include(includeProperty);
                    }
                    return queryable;

                });
        }

        #endregion
    }
}
