using System;
using System.Threading.Tasks;
using System.Threading;

namespace TastAndWait
{
    class Program
    {
        static void Main(string[] args)
        {
            int ii = 5;
            Console.WriteLine("Hello World!"+DateTime.Now);
            // Task t = Task.Factory.StartNew(() => Console.ReadLine());
            bool flag = true;
            while(flag)
            {
                if(Console.KeyAvailable)
                {

                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch(key.Key)
                    {
                        case ConsoleKey.A:
                            Console.WriteLine("Glue continuing...");
                            flag = false;
                            break;
                        default:
                            Console.WriteLine("Press A to stop waiting.");
                            break;
                    }
 
                }
            }

       //     Task t2 = Task.Factory.StartNew((object time) => { ii= 9; Console.ReadKey(); Console.WriteLine(time.ToString()); },DateTime.Now);
            //Task.WaitAll(t2);
       //     t2.Wait();
            for(int i=0;i<ii;i++)
            {
                Console.WriteLine("hahahahah!" + DateTime.Now);
                Thread.Sleep(1000);
            }


        }
    }
}
