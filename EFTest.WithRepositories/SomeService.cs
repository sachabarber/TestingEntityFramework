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

        public void Insert(string url)
        {
            Post post = new Post() { Url = url };
            post.PostComments.Add(new PostComment()
            {
                Comment = string.Format("yada yada {0}", counter++)
            });
            repository.Add(post);
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

        public Task<bool> InsertAsync(string url)
        {
            Post post = new Post() { Url = url };
            post.PostComments.Add(new PostComment()
            {
                Comment = string.Format("yada yada {0}", counter++)
            });
            return repository.AddAsync(post);
        }

        public async Task<List<Post>> GetAllAsync()
        {
            var posts = await repository.GetAllAsync();
            return posts.ToList();
        }

        public Task<Post> FindByIdAsync(int id)
        {
            return repository.GetIncludingAsync(id, x => x.PostComments);

        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
