﻿using System;
using System.Collections.Generic;
using System.Text;
using BlaBlaClient;
using Common;
using Common.ICommandPattern;

namespace Client.Commands
{
    public class UsersCommand : ICommand, ICommandFactory
    {

        public PackageManager Manager { get; set; }
        public DataPackage Cmd { get; set; }
        public PackageTypeEnum Type { get => PackageTypeEnum.Users; }


        public void Execute()
        {
            if (Cmd.Content is User)
            {
                Manager.Settings.ActiveUsers = Cmd.Content as List<User>;
                Manager.UsersListReceived?.Invoke(Cmd.Content as List<User>);
                Console.WriteLine("Get users");
            }
        }

        ICommand ICommandFactory.MakeCommand(DataPackage Cmd, PackageManager manager)
            =>  new UsersCommand() { Cmd = Cmd, Manager = manager };
        
    }
}