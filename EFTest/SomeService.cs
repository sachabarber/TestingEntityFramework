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
        private readonly ISachaContext context;
        private int counter;

        public SomeService(ISachaContext context)
        {
            this.context = context;
        }


        public void Insert(string url)
        {
            Post post = new Post() { Url = url };
            post.PostComments.Add(new PostComment()
            {
                Comment = string.Format("yada yada {0}", counter++)
            });
            context.Posts.Add(post);
        }

        public IEnumerable<Post> GetAll()
        {
            return context.Posts.AsEnumerable();
        }


        public IEnumerable<Post> GetAll(Expression<Func<Post, bool>> filter)
        {
            return context.Posts.Where(filter).AsEnumerable();
        }

        public Post FindById(int id)
        {
            //NOTE : Even if you included a line like the one below it would include 
            //the PostComments, which seems to be NonLazy
            //this is due to the fact that the Post(s) and Comment(s) are already in the Context
            //var post1 = context.Posts.FirstOrDefault(p => p.Id == id);

            //This should show that we are not doing Lazy Loading and DO NEED to use 
            //Include for navigation properties
            var postWithNoCommentsProof = context.Posts.FirstOrDefault();
            var postWithCommentsThanksToInclude = context.Posts
                .Include(x => x.PostComments).FirstOrDefault();

            var post = context.Posts.Where(p => p.Id == id)
                .Include(x => x.PostComments).FirstOrDefault();
            return post;
        }

        public async Task<bool> InsertAsync(string url)
        {
            Post post = new Post() { Url = url };
            post.PostComments.Add(new PostComment()
            {
                Comment = string.Format("yada yada {0}", counter++)
            });
            context.Posts.Add(post);
            return true;
        }

        public async Task<List<Post>> GetAllAsync()
        {
            return await context.Posts.ToListAsync(); 
        }


        public async Task<Post> FindByIdAsync(int id)
        {
            //NOTE : Even if you included a line like the one below it would include 
            //the PostComments, which seems to be NonLazy
            //this is due to the fact that the Post(s) and Comment(s) are already in the Context
            //var post1 = context.Posts.FirstOrDefault(p => p.Id == id);

            //This should show that we are not doing Lazy Loading and DO NEED to use 
            //Include for navigation properties
            var postWithNoCommentsProof = await context.Posts.FirstOrDefaultAsync();
            var postWithCommentsThanksToInclude = await context.Posts
                .Include(x => x.PostComments).FirstOrDefaultAsync();

            var post = await context.Posts.Where(p => p.Id == id)
                .Include(x => x.PostComments).FirstOrDefaultAsync();
            return post;
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
