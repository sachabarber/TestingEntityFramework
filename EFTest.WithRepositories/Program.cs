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

        public async void Run()
        {

            using (var scope = IOCManager.Instance.Container.BeginLifetimeScope())
            {
                ISomeService someService = scope.Resolve<ISomeService>();


                await someService.InsertAsync();
                var posts = await someService.GetAllAsync();
                var post = await someService.FindByIdAsync(posts.Last().Id);

                Console.ReadLine();

            }
        }




        static void Main(string[] args)
        {
            Program p = new Program();
            p.Run();
        }
    }
}
