using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EFTest.Entities;
using Moq;
using NUnit.Framework;

namespace EFTest.WithRepositories.Tests
{
    [TestFixture]
    public class SomeServiceTests
    {

        [TestCase]
        public void TestInsert()
        {
            Mock<IUnitOfWork> uowMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Post>> repoMock = new Mock<IRepository<Post>>();

            SomeService service = new SomeService(uowMock.Object, repoMock.Object);
            service.Insert();

            repoMock.Verify(m => m.Add(It.IsAny<Post>()), Times.Once());
            uowMock.Verify(m => m.Commit(), Times.Once());
        }


        [TestCase]
        public void TestGetAll()
        {
            Mock<IUnitOfWork> uowMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Post>> repoMock = new Mock<IRepository<Post>>();

            var posts = Enumerable.Range(0, 5).Select(x => new Post()
                                                           {
                                                               Url = string.Format("www.someurl{0}", x)
                                                           }).ToList();
            repoMock.Setup(x => x.GetAll()).Returns(posts.AsQueryable());

            SomeService service = new SomeService(uowMock.Object, repoMock.Object);
            var retrievedPosts  = service.GetAll();

            repoMock.Verify(m => m.GetAll(), Times.Once());

            CollectionAssert.AreEqual(posts, retrievedPosts);
        }

        [TestCase]
        public void TestGetAllWithLambda()
        {
            Mock<IUnitOfWork> uowMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Post>> repoMock = new Mock<IRepository<Post>>();

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

            repoMock.Setup(moq => moq.GetAll(It.IsAny<Expression<Func<Post, bool>>>()))
                    .Returns((Expression<Func<Post, bool>> predicate) => 
                        posts.Where(predicate.Compile()).AsQueryable());

            SomeService service = new SomeService(uowMock.Object, repoMock.Object);

            Func<Post, bool> func = (x) => x.Url == "www.someurl1";
            Expression<Func<Post, bool>> filter = post => func(post);

            var retrievedPosts = service.GetAll(filter);
            CollectionAssert.AreEqual(posts.Where(func), retrievedPosts);
        }



        [TestCase]
        public void TestFindById()
        {
            Mock<IUnitOfWork> uowMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Post>> repoMock = new Mock<IRepository<Post>>();

            var posts = Enumerable.Range(0, 5).Select(x => new Post()
            {
                Id = x,
                Url = string.Format("www.someurl{0}", x)
            }).ToList();

            for (int i = 0; i < posts.Count; i++)
            {
                posts[i].PostComments.Add(new PostComment() { Comment = string.Format("some test comment {0}", i) });
            }

            repoMock.Setup(moq => moq.Get(It.IsInRange(0, 5, Range.Inclusive)))
                .Returns((int id) => posts.SingleOrDefault(x => x.Id == id));


            SomeService service = new SomeService(uowMock.Object, repoMock.Object);
            var retrievedPost = service.FindById(2);


            Assert.AreEqual(2, retrievedPost.Id);
        }



        [TestCase]
        public async void TestInsertAsync()
        {
            Mock<IUnitOfWork> uowMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Post>> repoMock = new Mock<IRepository<Post>>();
            repoMock.Setup(x => x.AddAsync(It.IsAny<Post>())).Returns(Task.FromResult(true));

            SomeService service = new SomeService(uowMock.Object, repoMock.Object);
            await service.InsertAsync();

            repoMock.Verify(m => m.AddAsync(It.IsAny<Post>()), Times.Once());
            uowMock.Verify(m => m.Commit(), Times.Once());
        }


        [TestCase]
        public async void TestGetAllAsync()
        {
            Mock<IUnitOfWork> uowMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Post>> repoMock = new Mock<IRepository<Post>>();

            var posts = Enumerable.Range(0, 5).Select(x => new Post()
            {
                Id = x,
                Url = string.Format("www.someurl{0}", x)
            }).ToList();

            repoMock.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(posts.AsQueryable()));

            SomeService service = new SomeService(uowMock.Object, repoMock.Object);
            var retrievedPosts = await service.GetAllAsync();

            repoMock.Verify(m => m.GetAllAsync(), Times.Once());

            CollectionAssert.AreEqual(posts, retrievedPosts);
        }




        [TestCase]
        public async void TestFindByIdAsync()
        {
            Mock<IUnitOfWork> uowMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Post>> repoMock = new Mock<IRepository<Post>>();

            var posts = Enumerable.Range(0, 5).Select(x => new Post()
            {
                Id = x,
                Url = string.Format("www.someurl{0}", x)
            }).ToList();

            for (int i = 0; i < posts.Count; i++)
            {
                posts[i].PostComments.Add(new PostComment() { Comment = string.Format("some test comment {0}", i) });
            }

            repoMock.Setup(moq => moq.GetIncludingAsync(
                        It.IsInRange(0, 5, Range.Inclusive), 
                        new[] { It.IsAny<Expression<Func<Post, object>>>() }))
                    .Returns(
                        (int id, Expression<Func<Post, object>>[] includes) => 
                            Task.FromResult(posts.SingleOrDefault(x => x.Id == id)));


            SomeService service = new SomeService(uowMock.Object, repoMock.Object);
            var retrievedPost = await service.FindByIdAsync(2);


            Assert.AreEqual(2, retrievedPost.Id);
        }

    }
}
