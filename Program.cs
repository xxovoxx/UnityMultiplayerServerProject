using System;
using MyServer.Servers;

namespace MyServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server(23346);
            Console.WriteLine($"[info][{DateTime.Now.ToString()}]  Server started successfully !");
            Console.Read();//卡住主线程免得控制台文件给关了
        }
    }
}