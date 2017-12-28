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
        public ServerPackageManager Manager { get; set; }
        public PackageTypeEnum Type { get => PackageTypeEnum.Users; }
        public TcpClient Client { get; set; }


        public void Execute()
        {
            DataPackage usersCmd = new DataPackage() { Type = PackageTypeEnum.Users, Content = Manager.Settings.Users };
            Manager.Communication.Send(Client, usersCmd);

        }

        ICommand ICommandFactory.MakeCommand(TcpClient Client, DataPackage Cmd, ServerPackageManager manager )
        {
            return new UsersCommand() { Cmd = Cmd, Manager = manager ,Client = Client };
        }
    }
}
