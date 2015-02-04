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
    public class SomeService : ISomeService, IDisposable
    {
        private readonly ISachaTestContext context;
        private int counter;

        public SomeService(ISachaTestContext context)
        {
            this.context = context;
        }


        public void Insert()
        {
            Post post = new Post() { Url = string.Format("www.someurl{0}", counter++) };
            post.PostComments.Add(new PostComment() { Comment = string.Format("yada yada {0}", counter++) });
            context.Posts.Add(post);
            context.SaveChanges();
        }

        public IEnumerable<Post> GetAll()
        {
            return context.Posts.AsEnumerable();
        }


        public IEnumerable<Post> GetAll(Expression<Func<Post, bool>> filter)
        {
            return context.Posts.Where(filter);
        }

        public Post FindById(int id)
        {
            //NOTE : Even if you included a line like the one below it would include the PostComments, which seems to be NonLazy
            //this is due to the fact that the Post(s) and Comment(s) are already in the Context
            //var post1 = context.Posts.FirstOrDefault(p => p.Id == id);

            //This should show that we are not doing Lazy Loading and DO NEED to use Include for navigation properties
            var postWithNoCommentsProof = context.Posts.FirstOrDefault();
            var postWithCommentsThanksToInclude = context.Posts.Include(x => x.PostComments).FirstOrDefault();

            var post = context.Posts.Where(p => p.Id == id).Include(x => x.PostComments).FirstOrDefault();
            return post;
        }

        public async Task InsertAsync()
        {
            Post post = new Post() { Url = string.Format("www.someurl{0}", counter++) };
            post.PostComments.Add(new PostComment() { Comment = string.Format("yada yada {0}", counter++) });
            context.Posts.Add(post);
            context.SaveChanges();
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return context.Posts.AsEnumerable();
        }


        public async Task<Post> FindByIdAsync(int id)
        {
            //NOTE : Even if you included a line like the one below it would include the PostComments, which seems to be NonLazy
            //this is due to the fact that the Post(s) and Comment(s) are already in the Context
            //var post1 = context.Posts.FirstOrDefault(p => p.Id == id);

            //This should show that we are not doing Lazy Loading and DO NEED to use Include for navigation properties
            var postWithNoCommentsProof = context.Posts.FirstOrDefault();
            var postWithCommentsThanksToInclude = context.Posts.Include(x => x.PostComments).FirstOrDefault();

            var post = context.Posts.Where(p => p.Id == id).Include(x => x.PostComments).FirstOrDefault();
            return post;
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
