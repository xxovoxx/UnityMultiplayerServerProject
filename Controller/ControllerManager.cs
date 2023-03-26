using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketProtocol;
using System.Reflection;
using MyServer.Servers;

namespace MyServer.Controller
{
    internal class ControllerManager//用于管理所有Controller
    {
        private Dictionary<RequestCode, BaseController> controlDict = new Dictionary<RequestCode, BaseController>();//建立字典
        private Server server;
        public ControllerManager(Server server)
        {
            this.server = server;

            UserController userController = new UserController();
            controlDict.Add(userController.GetRequestCode, userController);
        }

        public void HandleRequest(MainPack pack, Client client)//处理请求
        {
            if(controlDict.TryGetValue(pack.requestCode, out BaseController controller))
            {
                string metname=pack.actionCode.ToString();
                MethodInfo method = controller.GetType().GetMethod(metname);
                if(method == null)
                {
                    Console.WriteLine("没有找到对应的处理方法");
                    return;
                }
                object[] obj = new object[] { server, client, pack };
                object ret = method.Invoke(controller,obj);
                if(ret != null)
                {
                    client.Send(ret as MainPack);
                }
            }
            else
            {
                Console.WriteLine("没有找到对应的controller处理方法");
            }
        }
    }
}
