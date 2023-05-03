using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyServer.Enums;
using SocketProtocol;
using System.Collections.Generic;
using Google.Protobuf.Collections;

namespace MyServer.Servers
{
    internal class Room
    {
        //房间信息
        private RoomPack roominfo;

        ////房间内所有的客户端
        private List<Client> clientList = new List<Client>();

        public Room(Client client, RoomPack pack, int roomID)
        {
            roominfo = pack;
            pack.Id = roomID;
            clientList.Add(client);
            client.GetRoom = this;
        }

        //返回房间信息
        public RoomPack GetRoomInfo
        {
            get
            {
                roominfo.CurrentNum = clientList.Count;
                return roominfo;
            }
        }

        public RepeatedField<PlayerPack> GetPlayerInfo()
        {
            RepeatedField<PlayerPack> pack = new RepeatedField<PlayerPack>();
            foreach (Client client in clientList)
            {
                PlayerPack playerPack = new PlayerPack();
                playerPack.DisplayName = client.displayName;
                playerPack.Uid = client.UID;
                pack.Add(playerPack);
            }
            return pack;
        }

        public void BroadCast(Client sender, MainPack pack)
        {
            foreach (Client client in clientList)
            {
                //不广播发送给发送者自己
                if(Equals(client, sender))
                {
                    continue;
                }
                client.Send(pack);
            }
        }

        //加入房间和离开房间
        public void Join(Client client)
        {
            clientList.Add(client);
            client.GetRoom = this;
            MainPack pack = new MainPack();
            pack.actionCode = ActionCode.PlayerList;
            foreach(PlayerPack playerPack in GetPlayerInfo())
            {
                pack.playerPacks.Add(playerPack);
            }
            BroadCast(client, pack);
        }

        public void Exit(Client client)
        {
            clientList.Remove(client);
            client.GetRoom = null;
            MainPack pack = new MainPack();
            pack.actionCode = ActionCode.PlayerList;
            foreach (PlayerPack playerPack in GetPlayerInfo())
            {
                pack.playerPacks.Add(playerPack);
            }
            BroadCast(client, pack);
        }
    }
}