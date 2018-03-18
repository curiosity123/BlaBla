using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using BlaBlaServer;
using Common;
using Common.ICommandPattern;

namespace BlaBlaServer.Commands
{
    public class UsersCommand : ICommand, ICommandFactory
    {
        public DataPackage Cmd { get; set; }
        public PackageManager Manager { get; set; }
        public PackageTypeEnum Type { get => PackageTypeEnum.Users; }
        public TcpClient Client { get; set; }


        public void Execute()
        {
            DataPackage usersCmd;

            if (Manager.Settings.Users.Count > 0)
                usersCmd = new DataPackage() { Type = PackageTypeEnum.Users, Content = Manager.Settings.Users };
            else
                usersCmd = new DataPackage() { Type = PackageTypeEnum.Users, Content = null };

            Manager.Communication.Send(Client, usersCmd);
        }

        ICommand ICommandFactory.MakeCommand(TcpClient Client, DataPackage Cmd, PackageManager manager )
        {
            return new UsersCommand() { Cmd = Cmd, Manager = manager ,Client = Client };
        }
    }
}
