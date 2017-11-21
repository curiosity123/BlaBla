﻿using Common;
using Common.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace BlaBlaClient
{
    public class ClientCommandManager
    {
        ClientSettings Settings;
        TcpClientCommunication communication;
   
        public ClientCommandManager(ClientSettings data, TcpClientCommunication communication)
        {
            this.communication = communication;
            this.communication.PackageReceived += CommandProcessor;
            this.Settings = data;
        }

        internal void CommandProcessor(TcpClient client, Command cmd)
        {
            if (cmd.Type == PackageTypeEnum.Users)
            {
                Settings.ActiveUsers = cmd.Content as List<User>;
            }

            if (cmd.Type == PackageTypeEnum.Login && cmd.Content is User)
            {
                Settings.CurrentUser = cmd.Content as User;
                communication.StartSendingAlivePackage(Settings.CurrentUser);
                Console.WriteLine("You are logged in");
            }

            if (cmd.Type == PackageTypeEnum.Logout)
            {
                communication.StopSendingAlivePackage();
                Settings.CurrentUser = new User();
                Console.WriteLine("You are logout");
            }

            if (cmd.Type == PackageTypeEnum.Message)
            {
                Message msg = cmd.Content as Message;
                Console.WriteLine(msg.Sender.NickName + "# Wrote: " + msg.Text);
            }

            if (cmd.Type == PackageTypeEnum.Register)
            {
                User u = cmd.Content as User;
                if (u != null)
                    Console.WriteLine("User " + u.NickName + " was registered succesfully! :)");
                else
                    Console.WriteLine("User " + u.NickName + " was not registered, Try different nickname... :(");
            }
        }


        public void RegisterNewUser(User user)
        {
            Command cmd = new Command() { Type = PackageTypeEnum.Register, Content = user };
            communication.Send(cmd);
        }

        public void Login(User user)
        {
            Command cmd = new Command() { Type = PackageTypeEnum.Login, Content = user };
            communication.Send(cmd);
        }

        public void Logout()
        {
            Command cmd = new Command() { Type = PackageTypeEnum.Logout, Content = Settings.CurrentUser };
            communication.Send(cmd);
        }

        public void GetUsers()
        {
            Command cmd = new Command() { Type = PackageTypeEnum.Users, Content = Settings.CurrentUser };
            communication.Send(cmd);
        }

        public void Message(string text, List<User> users)
        {
            Message msg = new Common.Message() { Sender = Settings.CurrentUser, UserList = users, Text = text };
            Command cmd = new Command() { Type = PackageTypeEnum.Message, Content = msg };
            communication.Send(cmd);
        }
    }
}
