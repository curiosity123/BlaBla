﻿using Common;
using Common.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace BlaBlaClient
{
    class Program
    {
        static Client client;

        static void Main(string[] args)
        {
            Thread.Sleep(1000);
            client = Client.Create(new XmlSerialization(), "127.0.0.1", 8000);
            client.Run();

            ConsoleKeyInfo cmd;
          
            while (cmd.KeyChar != 'q')
            {

                Console.Clear();
                PrintMenu();
                cmd = Console.ReadKey();
                Console.WriteLine();

                if (cmd.KeyChar == 'r')
                    Register();

                if (cmd.KeyChar == 'l')
                    Login();

                if (cmd.KeyChar == 'o')
                    Logout();

                if (cmd.KeyChar == 'm')
                    NewMessage();

                if (cmd.KeyChar == 'u')
                    Users();

                if (cmd.KeyChar == 'c')
                    Conversation();

            }
        }

        private static void NewMessage()
        {
            Console.WriteLine("Enter message");
            string message = Console.ReadLine();
            Console.WriteLine("Enter consmer nick name");
            client.CommandManager.Message(message, client.Settings.ActiveUsers);    
        }

        private static void Users()
        {
            client.CommandManager.GetUsers();
        }

        private static void Conversation()
        {
            client.CommandManager.GetConversation(client.Settings.CurrentUser);
        }

        private static void Logout()
        {
            client.CommandManager.Logout();
        }

        private static void Login()
        {
            User usr = new User();
            Console.WriteLine("Enter nickname");
            usr.NickName = Console.ReadLine();
            Console.WriteLine("Enter password");
            usr.Password = Console.ReadLine();
            client.CommandManager.Login(usr);
        }

        private static void Register()
        {
            User usr = new User();
            Console.WriteLine("Enter nickname");
            usr.NickName = Console.ReadLine();
            Console.WriteLine("Enter password");
            usr.Password = Console.ReadLine();
            client.CommandManager.RegisterNewUser(usr);
        }




        private static void PrintMenu()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("r -register new user");
            Console.WriteLine("l -login");
            Console.WriteLine("o -logout");
            Console.WriteLine("m -send message");
            Console.WriteLine("u -get user list");
            Console.WriteLine("c -get conversation");
            Console.WriteLine("q -exit");
        }
    }

}
