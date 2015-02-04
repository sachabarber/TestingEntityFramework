using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;
using EFTest.Entities;
using Autofac;

namespace EFTest.WithRepositories
{
    public class SomeService : ISomeService, IDisposable
    {
        private readonly IUnitOfWork context;
        private readonly IRepository<Post> repository;

        private int counter;

        public SomeService(IUnitOfWork context, IRepository<Post> repository)
        {
            this.context = context;
            this.repository = repository;
        }

        public void Insert()
        {
            Post post = new Post() { Url = string.Format("www.someurl{0}", counter++) };
            post.PostComments.Add(new PostComment() { Comment = string.Format("yada yada {0}", counter++) });
            repository.Add(post);
            context.Commit();
        }

        public IEnumerable<Post> GetAll()
        {
            return repository.GetAll();
        }

        public IEnumerable<Post> GetAll(Expression<Func<Post, bool>> filter)
        {
            return repository.GetAll(filter);
        }

        public Post FindById(int id)
        {
            var post = repository.Get(id);
            return post;
        }

        public async Task InsertAsync()
        {
            Post post = new Post() { Url = string.Format("www.someurl{0}", counter++) };
            post.PostComments.Add(new PostComment() { Comment = string.Format("yada yada {0}", counter++) });
            await repository.AddAsync(post);
            context.Commit();

        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Post> FindByIdAsync(int id)
        {
            var post = await repository.GetIncludingAsync(id, x => x.PostComments);
            return post;

        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
