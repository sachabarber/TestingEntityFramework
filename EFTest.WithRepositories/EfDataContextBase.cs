using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFTest.WithRepositories
{
    public abstract class EfDataContextBase : DbContext, IUnitOfWork
    {

        public EfDataContextBase(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public IQueryable<T> Get<T>() where T : class
        {
            return Set<T>();
        }

        public bool Remove<T>(T item) where T : class
        {
            try
            {
                Set<T>().Remove(item);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public new int SaveChanges()
        {
            return base.SaveChanges();
        }

        public void Attach<T>(T obj) where T : class
        {
            Set<T>().Attach(obj);
        }

        public void Add<T>(T obj) where T : class
        {
            Set<T>().Add(obj);

            
        }
    }
}
