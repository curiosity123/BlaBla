using System;
using System.Collections.Generic;
using System.Text;
using BlaBlaClient;
using Client.ICommand;
using Common;

namespace Client.ICommand
{
    public class LogoutCommand : ICommand, ICommandFactory
    {
        public Command Cmd { get; set; }
        public ClientCommandManager Manager { get; set; }
        public PackageTypeEnum Type { get => PackageTypeEnum.Logout; }


        public void Execute()
        {

            Manager.LogoutPackageReceived?.Invoke();
            Manager.communication.StopSendingAlivePackage();
            Manager.Settings.CurrentUser = new User();
            Console.WriteLine("You are logout");


        }

        ICommand ICommandFactory.MakeCommand(Command Cmd, ClientCommandManager manager)
        {
            return new LogoutCommand() { Cmd = Cmd, Manager = manager };
        }
    }
}
