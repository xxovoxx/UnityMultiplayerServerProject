using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using MyServer.Tools;

namespace MyServer.Servers
{
    internal class Client//管理连接的客户端，每有一个客户端连接进来就会实例化一个
    {
        private Socket socket;
        private Message message;

        public Client(Socket socket)//构造函数
        {
            this.socket = socket;
            message = new Message();
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

                message.ReadBuffer(len);
                StartReceive();//解析完再次开始接收
            }
            catch
            {

            }
        }
    }
}