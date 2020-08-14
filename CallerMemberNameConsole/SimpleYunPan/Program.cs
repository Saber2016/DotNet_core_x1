using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace SimpleYunPan
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }



    public class HttpFileDescriptor
    {
        public bool Exists { set; get; }
        public bool IsDirectory { set; get; }
        public DateTimeOffset LastModified { get; set; }
        public long Length { get; set; }
        public string Name { get; set; }
        public string PhysicalPath { get; set; }

        public HttpFileDescriptor() { }
        public HttpFileDescriptor(IFileInfo fileInfo, Func<string, string> physicalPathResolver)
        {
            this.Exists = fileInfo.Exists;
            IsDirectory = fileInfo.IsDirectory;
            LastModified = fileInfo.LastModified;
            Length = fileInfo.Length;
            Name = fileInfo.Name;
            PhysicalPath = physicalPathResolver(fileInfo.Name);
        }

        public IFileInfo ToFileInfo(HttpClient httpClient)
        {
            return Exists ? new HttpFileInfo(this, httpClient) : (IFileInfo)new NotFoundFileInfo(this.Name);
        }
    }

    public class HttpFileInfo:IFileInfo
    {
        private HttpClient _httpClient;

        public bool Exists { set; get; }
        public bool IsDirectory { set; get; }
        public DateTimeOffset LastModified { get; set; }
        public long Length { get; set; }
        public string Name { get; set; }
        public string PhysicalPath { get; set; }

       // public bool Exists => throw new NotImplementedException();

        public HttpFileInfo(HttpFileDescriptor descriptor,HttpClient httpClient)
        {
            this.Exists = descriptor.Exists;
            IsDirectory = descriptor.IsDirectory;
            LastModified = descriptor.LastModified;
            Length = descriptor.Length;
            Name = descriptor.Name;
            PhysicalPath = descriptor.PhysicalPath;
            _httpClient = httpClient;
        }
        public Stream CreateReadStream()
        {
            HttpResponseMessage message = _httpClient.GetAsync(this.PhysicalPath).Result;
            return message.Content.ReadAsStreamAsync().Result;
        }


    }

    public class HttpDirectoryContentDescriptor
    {
        public bool Exits { get; set; }
        public IEnumerable<HttpFileDescriptor> fileDescriptors { get; set; }

        public HttpDirectoryContentDescriptor()
        {
            fileDescriptors = new HttpFileDescriptor[0];
        }

        public HttpDirectoryContentDescriptor(IDirectoryContents directoryContents,Func<string,string> physicalPathResolver)
        {
            Exits = directoryContents.Exists;
            fileDescriptors = directoryContents.Select(_ => new HttpFileDescriptor(_, physicalPathResolver));
        }



    }

    public class HttpDirectoryContents:IDirectoryContents
    {
        private IEnumerable<IFileInfo> _fileInfos;
        public bool Exists { get; set; }

        public HttpDirectoryContents(HttpDirectoryContentDescriptor descriptor,HttpClient httpClient)
        {
            Exists = descriptor.Exits;
            _fileInfos = descriptor.fileDescriptors.Select(file => file.ToFileInfo(httpClient));
        }

        public IEnumerator<IFileInfo> GetEnumerator() => _fileInfos.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _fileInfos.GetEnumerator();

    }

    public class HttpFileProvider : IFileProvider
    {
        private readonly string _baseAddress;
        private HttpClient _httpClient;
    
       public HttpFileProvider(string baseAddress)
       {
            _baseAddress = baseAddress.TrimEnd('/');
          _httpClient     = new HttpClient();
     }
       public IDirectoryContents GetDirectoryContents(string subpath)
     {
         string url = $"{_baseAddress}/{subpath.TrimStart('/')}?dir-meta";
         string content = _httpClient.GetStringAsync(url).Result;
         HttpDirectoryContentDescriptor descriptor = JsonConvert.DeserializeObject<HttpDirectoryContentDescriptor>(content);
         return new HttpDirectoryContents(descriptor, _httpClient);
     }
  
     public IFileInfo GetFileInfo(string subpath)
     {
         string url = $"{_baseAddress}/{subpath.TrimStart('/')}?file-meta";
         string content = _httpClient.GetStringAsync(url).Result;
        HttpFileDescriptor descriptor = JsonConvert.DeserializeObject<HttpFileDescriptor>(content);
         return descriptor.ToFileInfo(_httpClient);
     }
       public IChangeToken Watch(string filter)
        {
         return NullChangeToken.Singleton;
     }
 }


}
