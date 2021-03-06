﻿using BlaBlaClient;
using Common;
using Common.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;


namespace BlaBlaConsoleClient
{
    class Program
    {
        static BlaBlaClient.Client client;

        static void Main(string[] args)
        {
            Thread.Sleep(1000);
            client = BlaBlaClient.Client.Create(new XmlSerialization(), "127.0.0.1", 8000);
            client.Run();

            ConsoleKeyInfo cmd;
            Console.ForegroundColor = ConsoleColor.Green;
            while (cmd.KeyChar != 'q')
            {

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                PrintMenu();
                Console.ForegroundColor = ConsoleColor.Cyan;
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

            }
            Console.WriteLine("Bye bye!");
        }

        private static void NewMessage()
        {
            Console.WriteLine("Enter message");
            string message = Console.ReadLine();
            Console.WriteLine("Enter consumer nick name");
            client.packageManager.Message(message, client.settings.ActiveUsers);    
        }

        private static void Users()
        {
            client.packageManager.GetUsers();
        }


        private static void Logout()
        {
            client.packageManager.Logout();
        }

        private static void Login()
        {
            User usr = new User();
            Console.WriteLine("Enter nickname");
            usr.NickName = Console.ReadLine();
            Console.WriteLine("Enter password");
            usr.Password = Console.ReadLine();
            client.packageManager.Login(usr);
        }

        private static void Register()
        {
            User usr = new User();
            Console.WriteLine("Enter nickname");
            usr.NickName = Console.ReadLine();
            Console.WriteLine("Enter password");
            usr.Password = Console.ReadLine();
            client.packageManager.RegisterNewUser(usr);
        }




        private static void PrintMenu()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("r register new user");
            Console.WriteLine("l login");
            Console.WriteLine("o logout");
            Console.WriteLine("m send message");
            Console.WriteLine("u get user list");
            Console.WriteLine("q exit");
        }
    }

}
