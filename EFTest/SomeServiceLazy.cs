using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;
using EFTest.Entities;
using Autofac;

namespace EFTest
{
    public class SomeServiceLazy : ISomeServiceLazy, IDisposable
    {
        private readonly ISachaTestLazyContext context;
        private int counter;

        public SomeServiceLazy(ISachaTestLazyContext context)
        {
            this.context = context;
        }


        public void Insert()
        {
            PostLazy post = new PostLazy() { Url = string.Format("www.someurl{0}", counter++) };
            post.PostComments.Add(new PostComment() { Comment = string.Format("yada yada {0}", counter++) });
            context.Posts.Add(post);
            context.SaveChanges();
        }

        public IEnumerable<PostLazy> GetAll()
        {
            return context.Posts.AsEnumerable();
        }

        public IEnumerable<PostLazy> GetAll(Expression<Func<PostLazy, bool>> filter)
        {
            return context.Posts.Where(filter);
        }

        public PostLazy FindById(int id)
        {
            var post = context.Posts.FirstOrDefault(p => p.Id == id);
            return post;
        }

        public async Task InsertAsync()
        {
            PostLazy post = new PostLazy() { Url = string.Format("www.someurl{0}", counter++) };
            post.PostComments.Add(new PostComment() { Comment = string.Format("yada yada {0}", counter++) });
            context.Posts.Add(post);
            context.SaveChanges();
        }

        public async Task<IEnumerable<PostLazy>> GetAllAsync()
        {
            return context.Posts.AsEnumerable();
        }

        public async Task<PostLazy> FindByIdAsync(int id)
        {
            var post = context.Posts.FirstOrDefault(p => p.Id == id);
            return post;
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
