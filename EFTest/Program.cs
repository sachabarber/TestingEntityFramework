using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autofac;

namespace EFTest
{
    class Program
    {

        public async Task Run()
        {

            ////Non lazy service examples
            //using (var scope = IOCManager.Instance.Container.BeginLifetimeScope())
            //{
            //    ISomeService someService = scope.Resolve<ISomeService>();
            //    await someService.InsertAsync();
            //    var posts = await someService.GetAllAsync();
            //    var postLast = await someService.FindByIdAsync(posts.Last().Id);
            //}


            //Lazy service examples
            using (var scope = IOCManager.Instance.Container.BeginLifetimeScope())
            {
                ISomeServiceLazy someServiceLazy = scope.Resolve<ISomeServiceLazy>();
                await someServiceLazy.InsertAsync(string.Format("EFTest {0}", DateTime.Now.ToLongTimeString()));
                var posts = await someServiceLazy.GetAllAsync();
                var postLast = await someServiceLazy.FindByIdAsync(posts.Last().Id);
            }


            Console.ReadLine();
        }

       


        static void Main(string[] args)
        {
            Program p = new Program();
            p.Run().Wait();
        }
    }
}
