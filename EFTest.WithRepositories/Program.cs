using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autofac;



namespace EFTest.WithRepositories
{
    class Program
    {

        public async Task Run()
        {

            using (var scope = IOCManager.Instance.Container.BeginLifetimeScope())
            {
                using (var uow = scope.Resolve<IUnitOfWork>())
                {
                    ISomeService someService = scope.Resolve<ISomeService>();

                    await someService.InsertAsync(string.Format("EFTest.WithRepositories {0}", DateTime.Now.ToLongTimeString()));
                    var posts = await someService.GetAllAsync();
                    var post = await someService.FindByIdAsync(posts.Last().Id);
                    uow.SaveChanges();
                }
                Console.WriteLine("DONE");
                Console.ReadLine();
            }
        }




        static void Main(string[] args)
        {
            Program p = new Program();
            p.Run().Wait();
        }
    }
}
