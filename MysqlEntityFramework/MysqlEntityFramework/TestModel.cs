using System;
using MysqlEntityFramework.test1;

namespace MysqlEntityFramework
{
    public class TestModel
    {

        public static readonly test_1Context test_1Context = new test_1Context();

        public static void AddItem(object item)
        {
            try
            {
                test_1Context.Add(item);
                test_1Context.SaveChanges();
                Console.WriteLine("添加成功！");
            }
            catch(Exception e)
            {
                Console.WriteLine($"操作失败！  {e.Message}");
            }
        }

        public static void DeleteItem(Type type, string id)
        {
            try
            {
                var ans = test_1Context.Find(type, id);
                test_1Context.Remove(ans);
                test_1Context.SaveChanges();
                Console.WriteLine("Operation Successed!");
            }
            catch(Exception e)
            {
                Console.WriteLine($"Operation failed! -------{e.Message}");
            }
        }
            
    }
}
