using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketProtocol;
using ProtoBuf;
using ProtoBuf.Meta;
using System.IO;

namespace MyServer.Tools
{
    internal class Message
    {
        private byte[] buffer = new byte[1024];

        private int startIndex;//当前buffer存到了第几位

        public byte[] Buffer//外界调用
        {
            get
            {
                return buffer;
            }
        }

        public int StartIndex
        {
            get
            {
                return startIndex;
            }
        }

        public int Remsize//buffer剩余的空间
        {
            get
            {
                return buffer.Length - startIndex;
            }
        }

        public void ReadBuffer(int len, Action<MainPack> HandleRequest)//读取消息 传入消息长度  第二个参数的回调函数是调用Client那边的函数
        {
            startIndex += len;
            if (startIndex <= 4) return;//数据包的包头，前四个字节是int类型，储存了包体的数据长度,如果长度小于等于4说明包不完整
            int count = BitConverter.ToInt32(buffer, 0);//解析前四个字节为int类型
            while(true)
            {
                if (startIndex >= (count + 4))
                {
                    MainPack pack = Serializer.Deserialize<MainPack>(new MemoryStream(buffer, 4, count));
                    HandleRequest(pack);
                    Array.Copy(buffer, count + 4, buffer, 0, startIndex - count - 4);//将包头和包体后面的部分拷贝到buffer的最前面，从而读取下一个包
                    startIndex -= (count + 4);
                }
                else
                {
                    break;
                }
            }
        }

        public static byte[] PackData(MainPack pack)//包装数据
        {
            var stream = new MemoryStream();
            Serializer.Serialize<MainPack>(stream, pack);
            byte[] data = stream.ToArray();//包体
            byte[] head = BitConverter.GetBytes(data.Length);//包头
            return head.Concat(data).ToArray();//合在一起返回
        }
    }
}
