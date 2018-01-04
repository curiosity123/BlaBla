﻿using Common;
using System;
using System.Collections.Generic;
using Client;
using Client.Commands;

namespace BlaBlaClient
{
    public class PackageManager
    {
        public readonly Settings Settings;
        public readonly Communication Communication;
        private readonly CommandParser PackageReceivedParser;


        public Action<Message> MessageReceived { get; set; }
        public Action<List<User>> UsersListReceived { get; set; }
        public Action<bool> RegistrationResultReceived { get; set; }
        public Action LogoutPackageReceived { get; set; }
        public Action<bool> LoginReceived { get; set; }




        public PackageManager(Settings data, Communication communication)
        {
            Communication = communication;
            Communication.PackageReceived += PackageProcessor;
            Settings = data;
            PackageReceivedParser = new CommandParser(GetAvailableCommands());
        }

        private PackageManager() { }


        private static IEnumerable<ICommandFactory> GetAvailableCommands()
            => new ICommandFactory[]
            {
                    new LoginCommand(),
                    new LogoutCommand(),
                    new UsersCommand(),
                    new MessageCommand(),
                    new RegisterCommand(),
                    new UnsupportedCommand()
            };

        public void PackageProcessor(DataPackage cmd)
            => PackageReceivedParser.ParseCommand(cmd, this)?.Execute();



        public void RegisterNewUser(User user)
        {
            DataPackage cmd = new DataPackage() { Type = PackageTypeEnum.Register, Content = user };
            Communication.Send(cmd);
        }

        public void Login(User user)
        {
            DataPackage cmd = new DataPackage() { Type = PackageTypeEnum.Login, Content = user };
            Communication.Send(cmd);
        }

        public void Logout()
        {
            DataPackage cmd = new DataPackage() { Type = PackageTypeEnum.Logout, Content = Settings.CurrentUser };
            Communication.Send(cmd);
        }

        public void GetUsers()
        {
            DataPackage cmd = new DataPackage() { Type = PackageTypeEnum.Users, Content = Settings.CurrentUser };
            Communication.Send(cmd);
        }

        public void Message(string text, List<User> users)
        {
            Message msg = new Common.Message() { Sender = Settings.CurrentUser, UserList = users, Text = text };
            DataPackage cmd = new DataPackage() { Type = PackageTypeEnum.Message, Content = msg };
            Communication.Send(cmd);
        }
    }
}
