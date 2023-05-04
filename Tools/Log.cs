using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace MyServer.Tools
{
    public class Log
    {
        static string path = "D:\\log.log";
        string[] states = { "info", "wrong", "info" };
        public Log()
        {
            Console.WriteLine($"[info][{ DateTime.Now.ToString()}]  Log started successfully in {path}");
        }
        public void make_log(int n, string _message)
        {
            Console.WriteLine($"[{states[n]}][{DateTime.Now.ToString()}]  {_message}");
            using (StreamWriter fw = File.AppendText(path))
            {
                fw.WriteLine($"[{states[n]}][{DateTime.Now.ToString()}]  {_message}");
                fw.Close();
            }
            
            return;
        }
    }
}
