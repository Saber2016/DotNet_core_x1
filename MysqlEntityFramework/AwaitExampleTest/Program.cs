using System;
using System.Threading.Tasks;
using System.Threading;

namespace AwaitExampleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var task = DoTaskWithAsync();
            //  task.Start();
            // Console.ReadLine();
            Console.WriteLine($"TIME :{DateTime.Now.ToShortDateString()}");
           // task.Wait();
            Console.WriteLine($"TIME :{DateTime.Now.ToShortDateString()}");
        }

        public static async Task DoTaskWithAsync()
        {
            Console.WriteLine("Await Taskfunction Start");
            await Task.Run(() =>
            {
                DoTaskFun();
            });
        }

        public static void DoTaskFun()
        {
            for(int i=0;i<5;i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine($"Task {i} has been Done! {Thread.CurrentThread.ManagedThreadId}");
            }
        }
    }
}
