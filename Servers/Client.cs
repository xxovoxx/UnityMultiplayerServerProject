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
using MySql.Data.MySqlClient;

namespace MyServer.Servers
{
    //管理连接的客户端，每有一个客户端连接进来就会实例化一个
    internal class Client
    {
        private Socket socket;
        private Message message;
        private UserDAO userDAO;
        private Server server;

        //当前此客户端的房间
        public Room GetRoom
        {
            get;set;
        }

        //此客户端登录的账号
        public string account
        {
            get;set;
        }

        public string displayName;
        public int UID;

        public UserDAO GetUserDao
        {
            get { return userDAO; }
        }

        //构造函数
        public Client(Socket socket, Server server)
        {
            userDAO = new UserDAO();
            message = new Message();

            this.server = server;
            this.socket = socket;

            StartReceive();
        }

        //开始接收数据
        private void StartReceive()
        {
            socket.BeginReceive(message.Buffer,message.StartIndex,message.Remsize,SocketFlags.None,ReceiveCallback,null);
        }

        //解析消息
        private void ReceiveCallback(IAsyncResult iar)
        {
            //防止客户端非正常退出报错
            try
            {
                if (socket == null || socket.Connected == false) return;
                int len = socket.EndReceive(iar);
                if (len == 0)
                {
                    return;
                }

                message.ReadBuffer(len, HandleRequest);
                //解析完再次开始接收
                StartReceive();
            }
            catch
            {

            }
        }

        private void HandleRequest(MainPack pack)
        {
            server.HandleRequest(pack, this);
        }

        //给客户端发送消息
        public void Send(MainPack pack)
        {
            socket.Send(Message.PackData(pack));
        }

        public bool Register(MainPack pack)
        {
            return GetUserDao.Register(pack);
        }
        public bool Login(MainPack pack)
        {
            return GetUserDao.Login(pack);
        }

        public string GetPlayerDisplayName(string account)
        {
            return GetUserDao.GetPlayerDisplayName(account);
        }

        public int GetPlayerUID(string account)
        {
            return GetUserDao.GetPlayerUID(account);
        }
    }
}