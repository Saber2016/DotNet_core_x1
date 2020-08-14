using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Linq;


namespace DIStudy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }


    public class Cat
    {
        private static readonly ConcurrentDictionary<Type, Type> typeMappings =
            new ConcurrentDictionary<Type, Type>();


        public object GetService(Type serviceType)
        {
            Type type;
            if(!typeMappings.TryGetValue(serviceType,out type))
            {
                type = serviceType;
            }
            if(type.IsInterface||type.IsAbstract)
            {
                return null;
            }
            ConstructorInfo constructor = this.GetConstructor(type);
            if(null==constructor)
            {
                return null;
            }
            object[] arguments = constructor.GetParameters().Select(p => this.GetService(p.ParameterType)).ToArray();
            object service = constructor.Invoke(arguments);
            //服务对象创建之后，分别调用下面2个方法对服务对象实施属性注入和方法注入
            this.InitializeInjectedProperties(service);
            InvokeInjectedMethods(service);
            return service;


        }
        /// <summary>
        /// 此方法体现了我们采用的注入构造函数的选择策略：优先选择标注有InjecttionAttribute特性的构造函数，
        /// 如果不存在则选择第一个共有的构造函数。执行构造函数传入的参数是递归调用GetService方法根据参数类型获得的。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual ConstructorInfo GetConstructor(Type type)
        {
            ConstructorInfo[] constructors = type.GetConstructors();
            return constructors.FirstOrDefault(c => c.GetCustomAttribute<InjectionAttribute>() != null)
                ?? constructors.FirstOrDefault();
        }
        /// <summary>
        ///利用反射得到所有标注了InjectionAttribute特性的依赖属性
        /// 并对它们进行赋值，具体的属性值同样是以递归的形式调用Getservice方法针对属性类型获得。
        /// </summary>
        /// <param name="service"></param>
        protected virtual void InitializeInjectedProperties(object service)
        {
            PropertyInfo[] properties = service.GetType().GetProperties().
                Where(p => p.CanWrite && p.GetCustomAttribute<InjectionAttribute>() != null).ToArray();
            Array.ForEach(properties, p => p.SetValue(service, this.GetService(p.PropertyType)));
        }

        protected virtual void InvokeInjectedMethods(object service)
        {
            MethodInfo[] methods = service.GetType().GetMethods().
                Where(m => m.GetCustomAttribute<InjectionAttribute>() != null).ToArray();
            Array.ForEach(methods, m =>
             {
                 object[] arguments = m.GetParameters().Select(p => GetService(p.ParameterType)).ToArray();
                 m.Invoke(service, arguments);
             });
        }


    }

    [AttributeUsage(AttributeTargets.All)]
    public sealed class InjectionAttribute :System.Attribute
    {
        public string desciption;
        public InjectionAttribute(string desc)
        {
            desciption = desc;
        }
    }

    
}
