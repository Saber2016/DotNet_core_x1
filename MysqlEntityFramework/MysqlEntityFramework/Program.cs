using System;
using MysqlEntityFramework.test1;


namespace MysqlEntityFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            NewTable new1 = new NewTable
            {
                Id = Guid.NewGuid().ToString(),
                Name = "sanmao",
                Age = 23,
                Gender = 1,
                Hight = 157,
                Weight = 50,
                Time = DateTime.Now
            };
            TestModel.AddItem(new1);
         //   TestModel.DeleteItem(typeof(NewTable), "1");
        }
    }
}
