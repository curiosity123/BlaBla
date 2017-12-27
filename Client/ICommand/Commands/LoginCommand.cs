using System;
using System.Collections.Generic;
using System.Text;
using BlaBlaClient;
using Client.ICommand;
using Common;

namespace Client.ICommand
{
    public class LoginCommand : ICommand, ICommandFactory
    {
        public Command Cmd { get; set; }
        public ClientCommandManager Manager { get; set; }
        public PackageTypeEnum Type { get => PackageTypeEnum.Login; }


        public void Execute()
        {
            if (Cmd.Content is User)
            {
                Manager.Settings.CurrentUser = Cmd.Content as User;
                Manager.communication.StartSendingAlivePackage(Manager.Settings.CurrentUser);
                Console.WriteLine("You are logged in");
                Manager.LogoutPackageReceived?.Invoke();
            }

        }

        ICommand ICommandFactory.MakeCommand(Command Cmd, ClientCommandManager manager )
        {
            return new LoginCommand() { Cmd = Cmd, Manager = manager };
        }
    }
}
