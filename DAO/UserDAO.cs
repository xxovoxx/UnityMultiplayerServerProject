using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MyServer.DAO
{
    internal class UserDAO
    {
        private MySqlConnection mySqlConnection;
        private string connStr = "database = socketgamedatabase; data source = 127.0.0.1;User ID = root ; Password = 123456; Pooling = true; Charset = utf8; Port = 3306";
        public UserDAO()
        {
            mySqlConnection = new MySqlConnection(connStr);
        }
    }
}