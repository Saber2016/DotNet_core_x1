using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.IO;
using System.Reflection;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

namespace fileSystem01
{
    class Program
    {
        static void Main(string[] args)
        {
            //  Console.WriteLine("Hello World!");
            /*
            new ServiceCollection().AddSingleton<IFileProvider>(new PhysicalFileProvider(@"C:\Users\95644\Documents\DotNet_core_x1\fileSystem01\fileSystem01"))
                  .AddSingleton<IFileManager, FileManager>()
                  .BuildServiceProvider().GetService<IFileManager>()
                  .ShowStructure((layer, name) =>
                  Console.WriteLine($"{new string('\t', layer)}{name}"));   
           string ans= new ServiceCollection().AddSingleton<IFileProvider>(new PhysicalFileProvider(@"C:\Users\95644\Documents\DotNet_core_x1\fileSystem01\fileSystem01"))
           .AddSingleton<IFileManager, FileManager>()
           .BuildServiceProvider().GetService<IFileManager>()
           .ReadAllTextAsync("data.txt").Result;
            Console.WriteLine(ans);   
            //利用EmbeddedFileprovider读取文件
            string ans = new ServiceCollection().AddSingleton<IFileProvider>(new PhysicalFileProvider(@"C:\Users\95644\Documents\DotNet_core_x1\fileSystem01\fileSystem01"))
                  .AddSingleton<IFileManager, FileManager>()
                  .BuildServiceProvider().GetService<IFileManager>()
                  .ReadAllTextAsync("data.txt").Result;
            Assembly assembly = Assembly.GetEntryAssembly();
            //直接读取内嵌资源文件
            Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.data.txt");
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            string content = Encoding.UTF8.GetString(buffer);
            Console.WriteLine($"{ans}\n{content}");   */

            //监控文件的变化
            IFileProvider fileProvider = new PhysicalFileProvider(@"C:\Users\95644\Documents\DotNet_core_x1\fileSystem01\fileSystem01");
            ChangeToken.OnChange(() => fileProvider.Watch("data.txt"), ()=>LoadFileAsync(fileProvider));
            while(true)
            {
                File.WriteAllText(@"C:\Users\95644\Documents\DotNet_core_x1\fileSystem01\fileSystem01\data.txt", DateTime.Now.ToString());
                Task.Delay(5000).Wait();
            }



        }
        public static async void LoadFileAsync(IFileProvider fileProvider)
        {
            Stream stream = fileProvider.GetFileInfo("data.txt").CreateReadStream();
            {
                byte[] buffer = new byte[stream.Length];
                await stream.ReadAsync(buffer, 0, buffer.Length);
                Console.WriteLine(Encoding.ASCII.GetString(buffer));
            }
        }



      
    }
    public interface IFileManager
    {
        /// <summary>
        /// 第一个餐数表示缩进，第二根参数表示要显示的文件或者目录
        /// </summary>
        /// <param name="render"></param>
        void ShowStructure(Action<int, string> render);

        Task<string> ReadAllTextAsync(string path);

    }

    public class FileManager : IFileManager
    {
        public IFileProvider fileProvider { get; private set; }

        public FileManager(IFileProvider provider)
        {
            fileProvider = provider;
        }

        public void ShowStructure(Action<int, string> render)
        {
            int layer = -1;
            Render("", ref layer, render);
        }

        public void Render(string subPath, ref int layer, Action<int, string> render)
        {
            layer++;
            foreach (var fileinfo in fileProvider.GetDirectoryContents(subPath))
            {
                render(layer, fileinfo.Name);
                if (fileinfo.IsDirectory)
                {
                    Render($@"{subPath}\{fileinfo.Name}".TrimStart('\\'), ref layer, render);
                }
            }
            layer--;
        }

        public async Task<string> ReadAllTextAsync(string path)
        {
            byte[] buffer;
            using (Stream readStream = fileProvider.GetFileInfo(path).CreateReadStream())
            {
                buffer = new byte[readStream.Length];
                await readStream.ReadAsync(buffer, 0, buffer.Length);
            }
            return Encoding.UTF8.GetString(buffer);
        }
    }
}
