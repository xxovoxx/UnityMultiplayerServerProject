using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketProtocol;
using MyServer.Servers;

namespace MyServer.Controller
{
    internal class UserController : BaseController
    {
        public UserController()
        {
            requestCode = RequestCode.User;
        }
        public MainPack Register(Server server, Client client, MainPack pack)//注册
        {
            if (server.Register(client, pack))
            {
                pack.returnCode = ReturnCode.Succeed;
            }
            else
            {
                pack.returnCode = ReturnCode.Failed;
            }
            return pack;
        }
        public MainPack Login(Server server, Client client, MainPack pack)//登录
        {

        }
    }
}
