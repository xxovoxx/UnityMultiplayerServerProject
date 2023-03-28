using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using MyServer.Tools;
using MyServer.DAO;
using SocketProtocol;

namespace MyServer.Servers
{
    internal class Client//管理连接的客户端，每有一个客户端连接进来就会实例化一个
    {
        private Socket socket;
        private Message message;
        private UserDAO userDAO;
        private Server server;

        public UserDAO GetUserDao
        {
            get { return userDAO; }
        }

        public Client(Socket socket, Server server)//构造函数
        {
            userDAO = new UserDAO();
            message = new Message();

            this.server = server;
            this.socket = socket;

            StartReceive();
        }

        private void StartReceive()//开始接收数据
        {
            socket.BeginReceive(message.Buffer,message.StartIndex,message.Remsize,SocketFlags.None,ReceiveCallback,null);
        }

        private void ReceiveCallback(IAsyncResult iar)//解析消息
        {
            try//防止客户端非正常退出报错
            {
                if (socket == null || socket.Connected == false) return;
                int len = socket.EndReceive(iar);
                if (len == 0)
                {
                    return;
                }

                message.ReadBuffer(len, HandleRequest);
                StartReceive();//解析完再次开始接收
            }
            catch
            {

            }
        }

        private void HandleRequest(MainPack pack)
        {
            server.HandleRequest(pack, this);
        }

        public void Send(MainPack pack)//给客户端发送消息
        {
            socket.Send(Message.PackData(pack));
        }

        public bool Register(MainPack pack)
        {
            return GetUserDao.Register(pack);
        }
    }
}