using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyServer.Enums;
using SocketProtocol;
using System.Collections.Generic;

namespace MyServer.Servers
{
    internal class Room
    {
        private RoomPack roominfo;

        private List<Client> clientList = new List<Client>();//房间内所有的客户端

        public Room(Client client, RoomPack pack)
        {
            roominfo = pack;
            clientList.Add(client);
        }

        //返回房间信息
        public RoomPack GetRoomInfo
        {
            get
            {
                return roominfo;
            }
        }
    }
}
