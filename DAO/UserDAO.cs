using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Mysqlx.Connection;
using SocketProtocol;

namespace MyServer.DAO
{
    internal class UserDAO
    {
        private MySqlConnection mySqlConnection;
        private string connStr = "database = socketgamedatabase; data source = 127.0.0.1;User ID = root ; Password = 123456; Pooling = true; Charset = utf8; Port = 3306";
        string sql;
        public UserDAO()
        {
            ConnectMySql();
        }

        private void ConnectMySql()
        {
            try
            {
                mySqlConnection = new MySqlConnection(connStr);
                mySqlConnection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine("连接数据库失败" + e.Message);
            }
        }

        public bool Register(MainPack pack)
        {
            string displayName = pack.registerPack.DisplayName;
            string account = pack.registerPack.Account;
            string password = pack.registerPack.Password;

            sql = "SELECT * FROM socketgamedatabase.userdata WHERE Account = @account";
            using (MySqlCommand command = new MySqlCommand(sql, mySqlConnection))
            {
                command.Parameters.AddWithValue("@account", account);

                using(MySqlDataReader read = command.ExecuteReader())
                {
                    if (read.HasRows)//如果查询到数据库已经有这个account后，返回真值，用户名已被注册
                    {
                        return false;
                    }
                }
            }

            password = GetMd5Hash(password);//加密密码

            sql = "INSERT INTO socketgamedatabase.userdata (DisplayName, Account,PasswordMD5) VALUES (@displayName, @account, @password)"; 
            using(MySqlCommand command = new MySqlCommand(sql, mySqlConnection))
            {
                command.Parameters.AddWithValue("@displayName", displayName);
                command.Parameters.AddWithValue("@account", account);
                command.Parameters.AddWithValue("@password", password);

                try
                {
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
        }

        static string GetMd5Hash(string input)//MD5加密算法
        {
            using (MD5 md5Hasher = MD5.Create())
            {
                byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }
    }
}