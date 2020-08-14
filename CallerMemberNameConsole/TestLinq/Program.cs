using System;
using System.Collections.Generic;
using System.Linq;

namespace TestLinq
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            List<simpleNum> ll = new List<simpleNum>();
            for(int i=1;i<10;i++)
            {
                simpleNum temp = new simpleNum(i);
                ll.Add(temp);
            }
            ll.ForEach(x => Console.WriteLine(x.num));
           // List<simpleNum> list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
            List<simpleNum> odd = ll.Where(x => x.num % 2 != 0).ToList();
           // var even = list.Where(x => x % 2 == 0).ToList();
            odd[0].num += 100;
            // Array.ForEach(list, new Action<int>(Console.WriteLine));
            // list.ForEach(x => Console.WriteLine(x));
            ll.ForEach(x => Console.WriteLine(x.num));

        }
    }
    class simpleNum
    {
        public int num;
        public simpleNum(int x) => num = x;
           
    }
}
