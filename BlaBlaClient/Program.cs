using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace BlaBlaClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(1000);
            Client client = new Client("127.0.0.1", 8000);
            client.Connect();
            client.RegisterNewUser(new User() {  NickName = "lukasz"});
          Thread.Sleep(1000);
            client.GetUsers();
                  Thread.Sleep(5000);
            client.Login(client.ActiveUsers[0]);
            Console.ReadKey();
        }
    }

}
