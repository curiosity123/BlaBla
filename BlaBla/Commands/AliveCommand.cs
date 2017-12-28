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
    public class AliveCommand : ICommand, ICommandFactory
    {
        public DataPackage Cmd { get; set; }
        public ServerPackageManager Manager { get; set; }
        public PackageTypeEnum Type { get => PackageTypeEnum.Alive; }
        public TcpClient Client { get; set; }


        public void Execute()
        {
            var session = (from x in Manager.Settings.Sessions
                           where x.User != null &&
                           x.User.Id == (Cmd.Content as User).Id
                           select x).FirstOrDefault();

            if (session != null)
                session.LastActivity = DateTime.UtcNow;

        }

        ICommand ICommandFactory.MakeCommand(TcpClient Client, DataPackage Cmd, ServerPackageManager manager )
        {
            return new AliveCommand() { Cmd = Cmd, Manager = manager ,Client = Client };
        }
    }
}
