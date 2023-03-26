using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using MyServer.Controller;
using SocketProtocol;

namespace MyServer.Servers
{
    internal class Server//监听客户端连接
    {
        private Socket socket;
        private List<Client> clientList = new List<Client>();//储存连接的客户端
        private ControllerManager controllerManager;

        public Server(int port)//用于main函数调用的构造函数，传入端口
        {
            controllerManager = new ControllerManager(this);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//初始化一个TCP协议的连接
            socket.Bind(new IPEndPoint(IPAddress.Any, port));//绑定IP和端口
            socket.Listen(0);//开始监听
            StartAccept();
        }

        private void StartAccept()//开始应答
        {
            socket.BeginAccept(AcceptCallback, null);
        }

        private void AcceptCallback(IAsyncResult iar)
        {
            Socket client = socket.EndAccept(iar);
            clientList.Add(new Client(client));
            StartAccept();//再次开始应答
        }


        public bool Register(Client client ,MainPack pack)
        {
           return client.GetUserDao.Register(pack);
        }
    }
}
