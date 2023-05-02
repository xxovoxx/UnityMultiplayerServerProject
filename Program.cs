using System;
using MyServer.Servers;

namespace MyServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server(23346);
            Console.WriteLine("服务器已启动");
            Console.Read();//卡住主线程免得控制台文件给关了
        }
    }
}