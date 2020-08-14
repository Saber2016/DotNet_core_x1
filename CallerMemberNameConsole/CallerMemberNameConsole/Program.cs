using System;
using System.Runtime.CompilerServices;

namespace CallerMemberNameConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            DoSomething();
        }


        public static void DoSomething()
        {
            TraceMessage("事情开始起变化!");
        }

        public static void TraceMessage(string message,
            [CallerMemberName] string memberName="",
            [CallerFilePath] string sourceFilePath="",
            [CallerLineNumber] int sourceLineNumber=0)
        {
            Console.WriteLine($"message:{message}\nmember name: {memberName}" +
                $"\nsource file path: {sourceFilePath}\nsource line number: {sourceLineNumber}");
        }
    }


}
