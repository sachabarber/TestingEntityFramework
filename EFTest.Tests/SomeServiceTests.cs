using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Threading.Tasks;
using EFTest.Entities;
using Moq;
using NUnit.Framework;

namespace EFTest.Tests
{

    /// <summary>
    /// This MSDN pages talks about how the EF team have made EF more testable
    /// https://msdn.microsoft.com/en-us/data/dn314429
    /// </summary>
    [TestFixture]
    public class SomeServiceTests
    {
        private static Mock<DbSet<T>> CreateMockSet<T>(IQueryable<T> dataForDbSet) where T : class
        {
            var dbsetMock = new Mock<DbSet<T>>();

            dbsetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(dataForDbSet.Provider);
            dbsetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(dataForDbSet.Expression);
            dbsetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(dataForDbSet.ElementType);
            dbsetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(dataForDbSet.GetEnumerator());
            return dbsetMock;
        }


        [TestCase]
        public void TestInsert()
        {
            var dbsetMock = new Mock<DbSet<Post>>();
            var uowMock = new Mock<SachaContextTestDouble>();
            uowMock.Setup(m => m.Posts).Returns(dbsetMock.Object); 

            var service = new SomeService(uowMock.Object);
            service.Insert("Some url");

            dbsetMock.Verify(m => m.Add(It.IsAny<Post>()), Times.Once()); 
        }


        [TestCase]
        public void TestGetAll()
        {

            var posts = Enumerable.Range(0, 5).Select(
                x => new Post()
                {
                    Url = string.Format("www.someurl{0}", x)
                }).AsQueryable();


            var dbsetMock = CreateMockSet(posts);

            var mockContext = new Mock<SachaContextTestDouble>();
            mockContext.Setup(c => c.Posts).Returns(dbsetMock.Object);



            var service = new SomeService(mockContext.Object);
            var retrievedPosts = service.GetAll();

            CollectionAssert.AreEqual(posts.ToList(), retrievedPosts.ToList());
        }

        [TestCase]
        public void TestGetAllWithLambda()
        {
            var posts = Enumerable.Range(0, 5).Select(x => new Post()
            {
                Url = string.Format("www.someurl{0}", x)
            }).ToList();

            for (int i = 0; i < posts.Count; i++)
            {
                posts[i].PostComments.Add(new PostComment()
                {
                    Comment = string.Format("some test comment {0}", i)
                });
            }

            var queryablePosts = posts.AsQueryable();

            var dbsetMock = CreateMockSet(queryablePosts);

            var mockContext = new Mock<SachaContextTestDouble>();
            mockContext.Setup(c => c.Posts).Returns(dbsetMock.Object);


            var service = new SomeService(mockContext.Object);

            Func<Post, bool> func = (x) => x.Url == "www.someurl1";
            Expression<Func<Post, bool>> filter = post => func(post);

            var retrievedPosts = service.GetAll(filter);
            CollectionAssert.AreEqual(posts.Where(func).ToList(), retrievedPosts.ToList());
        }



        [TestCase]
        public void TestFindById()
        {
            var posts = Enumerable.Range(0, 5).Select(x => new Post()
            {
                Id = x,
                Url = string.Format("www.someurl{0}", x)
            }).ToList();

            for (int i = 0; i < posts.Count; i++)
            {
                posts[i].PostComments.Add(new PostComment()
                {
                    Comment = string.Format("some test comment {0}", i)
                });
            }

            var queryablePosts = posts.AsQueryable();

            var dbsetMock = CreateMockSet(queryablePosts);

            //NOTE : we need to use the string version of Include as the other one that accepts
            //       an Expression tree is an extension method in System.Data.Entity.QueryableExtensions
            //       which Moq doesn't like
            //
            // So the following will not work, as will result in this sort of Exception from Moq
            //
            //       Expression references a method that does not belong to 
            //       the mocked object: m => m.Include<Post,IEnumerable`1>(It.IsAny<Expression`1>())
            //
            // dbsetMock.Setup(m => m.Include(It.IsAny<Expression<Func<Post,IEnumerable<PostComment>>>>()))
            //       .Returns(dbsetMock.Object);
            dbsetMock.Setup(m => m.Include("PostComments")).Returns(dbsetMock.Object);



            var mockContext = new Mock<SachaContextTestDouble>();
            mockContext.Setup(c => c.Posts).Returns(dbsetMock.Object);

            var service = new SomeService(mockContext.Object);
            var retrievedPost = service.FindById(1);

            Assert.AreEqual(retrievedPost.Id,1);
            Assert.IsNotNull(retrievedPost.PostComments);
            Assert.AreEqual(retrievedPost.PostComments.Count,1);
            
            

        }



        //[TestCase]
        //public async void TestInsertAsync()
        //{
        //    Mock<IUnitOfWork> uowMock = new Mock<IUnitOfWork>();
        //    Mock<IRepository<Post>> repoMock = new Mock<IRepository<Post>>();
        //    repoMock.Setup(x => x.AddAsync(It.IsAny<Post>())).Returns(Task.FromResult(true));

        //    SomeService service = new SomeService(uowMock.Object, repoMock.Object);
        //    await service.InsertAsync();

        //    repoMock.Verify(m => m.AddAsync(It.IsAny<Post>()), Times.Once());
        //    uowMock.Verify(m => m.Commit(), Times.Once());
        //}


        //[TestCase]
        //public async void TestGetAllAsync()
        //{
        //    Mock<IUnitOfWork> uowMock = new Mock<IUnitOfWork>();
        //    Mock<IRepository<Post>> repoMock = new Mock<IRepository<Post>>();

        //    var posts = Enumerable.Range(0, 5).Select(x => new Post()
        //    {
        //        Id = x,
        //        Url = string.Format("www.someurl{0}", x)
        //    }).ToList();

        //    repoMock.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(posts.AsQueryable()));

        //    SomeService service = new SomeService(uowMock.Object, repoMock.Object);
        //    var retrievedPosts = await service.GetAllAsync();

        //    repoMock.Verify(m => m.GetAllAsync(), Times.Once());

        //    CollectionAssert.AreEqual(posts, retrievedPosts);
        //}




        //[TestCase]
        //public async void TestFindByIdAsync()
        //{
        //    Mock<IUnitOfWork> uowMock = new Mock<IUnitOfWork>();
        //    Mock<IRepository<Post>> repoMock = new Mock<IRepository<Post>>();

        //    var posts = Enumerable.Range(0, 5).Select(x => new Post()
        //    {
        //        Id = x,
        //        Url = string.Format("www.someurl{0}", x)
        //    }).ToList();

        //    for (int i = 0; i < posts.Count; i++)
        //    {
        //        posts[i].PostComments.Add(new PostComment() { Comment = string.Format("some test comment {0}", i) });
        //    }

        //    repoMock.Setup(moq => moq.GetIncludingAsync(
        //                It.IsInRange(0, 5, Range.Inclusive), 
        //                new[] { It.IsAny<Expression<Func<Post, object>>>() }))
        //            .Returns(
        //                (int id, Expression<Func<Post, object>>[] includes) => 
        //                    Task.FromResult(posts.SingleOrDefault(x => x.Id == id)));


        //    SomeService service = new SomeService(uowMock.Object, repoMock.Object);
        //    var retrievedPost = await service.FindByIdAsync(2);


        //    Assert.AreEqual(2, retrievedPost.Id);
        //}

    }
}
