using System;
using System.Collections.Generic;
using System.Text;
using BlaBlaClient;
using Client.ICommand;
using Common;

namespace Client.ICommand
{
    public class UsersCommand : ICommand, ICommandFactory
    {

        public ClientCommandManager Manager { get; set; }
        public Command Cmd { get; set; }
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

        ICommand ICommandFactory.MakeCommand(Command Cmd, ClientCommandManager manager)
            =>  new UsersCommand() { Cmd = Cmd, Manager = manager };
        
    }
}
