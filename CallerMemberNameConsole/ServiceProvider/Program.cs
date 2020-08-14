using System;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceProvider
{


    class Program
    {
        static void Main(string[] args)
        {

            IServiceCollection services = new ServiceCollection()
                .AddSingleton<IFoo, Foo>()
                .AddSingleton<IBar>(new Bar())
                .AddSingleton<IBaz>(_ => new Baz())
                .AddSingleton<IGux, Gux>();

            IServiceProvider Provider = services.BuildServiceProvider();
            Console.WriteLine($"serviceProvider.GetService<IFoo>(): {Provider.GetService<IFoo>().GetType()}");
            Console.WriteLine("serviceProvider.GetService<IBar>(): {0}", Provider.GetService<IBar>().GetType().Name);
            Console.WriteLine($"serviceProvider.GetService<IBaz>(): {Provider.GetService<IBaz>()}");
            Console.WriteLine($"serviceProvider.GetService<IGux>(): {Provider.GetService<IGux>()}");
          //  Console.WriteLine("Hello World!");
        }
    }


    public interface IFoo { }
    public interface IBar { }
    public interface IBaz { }
    public interface IGux
    {
        IFoo Foo { get; }
        IBar Bar { get; }
        IBaz Baz { get; }
    }

    public class Foo : IFoo { }
    public class Bar : IBar { }
    public class Baz : IBaz { }
    public class Gux: IGux
    {
        public IFoo Foo { get; private set; }
        public IBar Bar { get; private set; }
        public IBaz Baz { get; private set; }

        public Gux(IFoo foo,IBar bar,IBaz baz)
        {
            Foo = foo;
            Bar = bar;
            Baz = baz;
        }
    }

}
