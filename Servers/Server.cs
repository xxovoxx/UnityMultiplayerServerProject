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
        private List<Room> roomList = new List<Room>();//储存房间

        private ControllerManager controllerManager;

        private int roomID = 0;

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
            clientList.Add(new Client(client, this));
            StartAccept();//再次开始应答
        }

        public void HandleRequest(MainPack pack, Client client)
        {
            controllerManager.HandleRequest(pack,client);
        }

        public MainPack CreateRoom(Client client, MainPack pack)
        {
            try
            {
                if (roomID > 2000000000)
                    roomID = 0;
                Room room = new Room(client, pack.roomPacks[0], roomID);
                roomList.Add(room);
                foreach(PlayerPack playerPack in room.GetPlayerInfo())
                {
                    pack.playerPacks.Add(playerPack);
                }
                roomID++;
                pack.returnCode = ReturnCode.Succeed;
                return pack;
            }
            catch
            {
                pack.returnCode = ReturnCode.Failed;
                return pack;
            }
        }

        public MainPack FindRoom()
        {
            MainPack pack = new MainPack();
            pack.actionCode = ActionCode.FindRoom;
            try
            {
                foreach (Room room in roomList)
                {
                    pack.roomPacks.Add(room.GetRoomInfo);
                }
                pack.returnCode = ReturnCode.Succeed;
            }
            catch
            {
                pack.returnCode = ReturnCode.Failed;
            }
            return pack;
        }

        public MainPack JoinRoom(Client client, MainPack pack)
        {
            foreach(Room room in roomList)
            {
                //是否有这个房间
                if (room.GetRoomInfo.Id.ToString().Equals(pack.Str))
                {
                    //房间是否可以加入
                    switch (room.GetRoomInfo.roomState)
                    {
                        //游戏中 不可加入
                        case RoomState.InGame:
                            pack.returnCode = ReturnCode.Failed;
                            break;
                        //游戏外 可加入
                        case RoomState.OutOfGame:
                            room.Join(client);
                            pack.roomPacks.Add(room.GetRoomInfo);
                            foreach (PlayerPack playerPack in room.GetPlayerInfo())
                            {
                                pack.playerPacks.Add(playerPack);
                            }
                            pack.returnCode = ReturnCode.Succeed;
                            break;
                    }
                    return pack;
                }
            }
            //遍历完了也没找到房间
            pack.returnCode = ReturnCode.Failed;
            return pack;
        }

        //退出房间
        public MainPack ExitRoom(Client client, MainPack pack)
        {
            //出现问题的情况
            if(client.GetRoom == null)
            {
                pack.returnCode = ReturnCode.Failed;
                return pack;
            }

            client.GetRoom.Exit(client);
            pack.returnCode = ReturnCode.Succeed;
            return pack;
        }
    }
}
