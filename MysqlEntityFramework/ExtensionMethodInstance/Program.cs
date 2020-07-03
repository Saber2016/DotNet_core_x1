using System;

namespace ExtensionMethodInstance
{
    class Program
    {
        static void Main(string[] args)
        {
            Data data = new Data(3, 4, 5);
         //   Console.WriteLine($"Average: {UglyWay.Average(data)}");

            Console.WriteLine(data.PrintInfo().Average());
         //   Console.WriteLine("Hello World!");
        }
    }

    public sealed class Data
    {
        public double d1, d2, d3;
        public Data(double D1,double D2,double D3)
        { 
            d1 = D1;
            d2 = D2;
            d3 = D3;
        }
        public double Sum()
        {
            return d1 + d2 + d3;
        }
    }

    public static class ExtendData
    {
        public static double Average(this Data data)
        {
            return data.Sum() / 3;
        }
        public static Data PrintInfo(this Data data)
        {
            Console.WriteLine($"d1: {data.d1}\td2: {data.d2}\td3: {data.d3}");
            return data;
        }
        //public static double Getd1(this Data data)
        //{
        //    return data.d1;
        //}

    }

    public class UglyWay
    {
        public static double Average(Data data)
        {
            return data.Sum() / 3;
        }
    }


}
