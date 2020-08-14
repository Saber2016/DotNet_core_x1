using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace FileProviderTest
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Assembly assembly = Assembly.GetEntryAssembly();
            String ans = new ServiceCollection().AddSingleton<IFileProvider>(new EmbeddedFileProvider(assembly))
    .AddSingleton<IFileManager, FileManager>()
    .BuildServiceProvider().GetService<IFileManager>().ReadAllTextAsync("data").Result;

            //直接读取内嵌资源文件
            Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.data");
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            string content = Encoding.UTF8.GetString(buffer);
            Console.WriteLine(content);
            Console.WriteLine(ans);
            Debug.Assert(content == ans);
            //String ans=new ServiceCollection().AddSingleton<IFileProvider>(new PhysicalFileProvider(@"/Users/qinyuanlong/DotNet_core_x1/CallerMemberNameConsole/FileProviderTest"))
            //    .AddSingleton<IFileManager, FileManager>()
            //    .BuildServiceProvider().GetService<IFileManager>().ReadAllTextAsync("data").Result;
            //Console.WriteLine(ans);
            //-----------------------
            // Debug.Assert(ans == File.ReadAllText(@"../data"));
            //  Console.WriteLine("Hello World!");
            //new ServiceCollection().AddSingleton<IFileProvider>(new PhysicalFileProvider(@"/Users/qinyuanlong/DotNet_core_x1/CallerMemberNameConsole/FileProviderTest"))
            //    .AddSingleton<IFileManager, FileManager>()
            //    .BuildServiceProvider().GetService<IFileManager>().ShowStructure(
            //    (layer, name) => Console.WriteLine("{0}{1}", new string('\t', layer), name));
            */
            //监控文件的变化
            IFileProvider fileProvider = new PhysicalFileProvider(@"/Users/qinyuanlong/DotNet_core_x1/CallerMemberNameConsole/FileProviderTest");
            ChangeToken.OnChange(() => fileProvider.Watch("data"), () => LoadFileAsync(fileProvider));
            while(true)
            {
                File.WriteAllText(@"/Users/qinyuanlong/DotNet_core_x1/CallerMemberNameConsole/FileProviderTest/data", DateTime.Now.ToString());
                Task.Delay(5000).Wait();
            }

        }

        public static async void LoadFileAsync(IFileProvider fileProvider)
        {
            Stream stream = fileProvider.GetFileInfo("data").CreateReadStream();
            {
                byte[] buffer = new byte[stream.Length];
                await stream.ReadAsync(buffer, 0, buffer.Length);
                Console.WriteLine(Encoding.ASCII.GetString(buffer));
            }
        }
    }


    public interface IFileManager
    {
        void ShowStructure(Action<int, string> render);
        Task<string> ReadAllTextAsync(string path);
    }


    public class FileManager:IFileManager
    {
        public IFileProvider fileProvider { get; private set; }

        public FileManager(IFileProvider _fileProvider)
        {
            fileProvider = _fileProvider;
        }

        public void ShowStructure(Action<int,string> render)
        {
            int layer = -1;//代表缩进的层级
            Render("",ref layer, render);
        }

        private void Render(string subPath,ref int layer,Action<int,string> render)
        {
            layer++;
            foreach(var fileinfo in this.fileProvider.GetDirectoryContents(subPath))
            {
                render(layer, fileinfo.Name);
                if(fileinfo.IsDirectory)
                {
                    Render($@"{subPath}\{fileinfo.Name}".TrimStart('\\'), ref layer, render);
                }
            }
        }

        public async Task<string> ReadAllTextAsync(string path)
        {
            byte[] buffer;
            using(Stream readStream=this.fileProvider.GetFileInfo(path).CreateReadStream())
            {
                buffer = new byte[readStream.Length];
                await readStream.ReadAsync(buffer, 0, buffer.Length);
            }
            return Encoding.UTF8.GetString(buffer);
        }

    }
}
