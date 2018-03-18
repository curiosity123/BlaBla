using System;
using System.Collections.Generic;
using System.Text;
using BlaBlaClient;
using Common;
using Common.ICommandPattern;

namespace Client.Commands
{
    public class LogoutCommand : ICommand, ICommandFactory
    {
        public DataPackage Cmd { get; set; }
        public PackageManager Manager { get; set; }
        public PackageTypeEnum Type { get => PackageTypeEnum.Logout; }


        public void Execute()
        {
            Manager.LogoutReceived?.Invoke();
            Manager.Communication.StopSendingAlivePackage();
            Manager.Settings.CurrentUser = new User();
            Console.WriteLine("You are logout");
        }

        ICommand ICommandFactory.MakeCommand(DataPackage Cmd, PackageManager manager)
        {
            return new LogoutCommand() { Cmd = Cmd, Manager = manager };
        }
    }
}
