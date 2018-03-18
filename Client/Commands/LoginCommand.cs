using System;
using System.Collections.Generic;
using System.Text;
using BlaBlaClient;
using Common;
using Common.ICommandPattern;

namespace Client.Commands
{
    public class LoginCommand : ICommand, ICommandFactory
    {
        public DataPackage Cmd { get; set; }
        public PackageManager Manager { get; set; }
        public PackageTypeEnum Type { get => PackageTypeEnum.Login; }


        public void Execute()
        {
            if (Cmd.Content is User)
            {
                Manager.Settings.CurrentUser = Cmd.Content as User;
                Manager.Communication.StartSendingAlivePackage(Manager.Settings.CurrentUser);
                Console.WriteLine("You are logged in");
                Manager.LoginReceived?.Invoke();
            }

        }

        ICommand ICommandFactory.MakeCommand(DataPackage Cmd, PackageManager manager )
        {
            return new LoginCommand() { Cmd = Cmd, Manager = manager };
        }
    }
}
