using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ConfigureStudy
{


    class Program
    {

        public static Dictionary<string, string> source = new Dictionary<string, string>
        {
            ["format:datetime:longDatePattern"] = "dddd, mmm d,yyy",
            ["format:dateTime:longTimePattern"] = "h:mm:ss tt",
            ["format:dateTime:shortDatePattern"] = "M/d/yyyy",
            ["format:dateTime:shortTimePattern"] = "h:mm tt",
            ["format:currencyDecimal:digits"]="2",
            ["format:currencyDecimal:symbol"]="$"

        };



        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
            .Add(new MemoryConfigurationSource { InitialData = source })
            .Build();
            FormatOptions options = new ServiceCollection()
                .AddOptions()
                .Configure<FormatOptions>(config.GetSection("Format"))
                .BuildServiceProvider()
                .GetService<IOptions<FormatOptions>>()
                .Value;
                

          //  FormatOptions options = new FormatOptions(config.GetSection("Format"));
            DateTimeFormatOption dateTimeFormatOption = options.DateTime;
            CurrencyDecimalOptions currencyDecimalOptions = options.CurrencyDecimal;
            Console.WriteLine($"LongDatePattern: {dateTimeFormatOption.LongDatePattern}");
            Console.WriteLine($"LongTimePattern:  {dateTimeFormatOption.LongTimePattern}");
            //  Console.WriteLine($"ShortDatePattern:  {options.ShortDatePattern}");
            //  Console.WriteLine($"ShortTimePattern:  { options.ShortTimePattern}");
            Console.WriteLine($"Digits: {currencyDecimalOptions.Digits}");
            Console.WriteLine($"symbol: {currencyDecimalOptions.Symbol}");
           // Console.WriteLine("Hello World!");
        }
    }


    public class DateTimeFormatOption
    {
        public string LongDatePattern { get; set; }
        public string LongTimePattern { get; set; }
        public string ShortDatePattern { get; set; }
        public string ShortTimePattern { get; set; }

        //public DateTimeFormatOption(IConfiguration config)
        //{
        //    LongDatePattern = config["LongDatePattern"];
        //    LongTimePattern = config["LongTimePattern"];
        //    ShortDatePattern = config["ShortDatePattern"];
        //    ShortTimePattern = config["ShortTimePattern"];
        //}
    }

    public class CurrencyDecimalOptions
    {
        public int Digits { get; set; }
        public string Symbol { get; set; }
        //public CurrencyDecimalOptions(IConfiguration config)
        //{
        //    Digits = int.Parse(config["Digits"]);
        //    Symbol = config["Symbol"];
        //}
    }
    public class FormatOptions
    {
        public DateTimeFormatOption DateTime { get; set; }
        public CurrencyDecimalOptions CurrencyDecimal { get; set; }

        //public FormatOptions(IConfiguration config)
        //{
        //    DateTime = new DateTimeFormatOption(config.GetSection("DateTime"));
        //    CurrencyDecimal = new CurrencyDecimalOptions(config.GetSection("CurrencyDecimal"));
        //}
    }

}
