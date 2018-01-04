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
    public class UnsupportedCommand : ICommand, ICommandFactory
    {
        public DataPackage Cmd { get; set; }
        public PackageManager Manager { get; set; }
        public PackageTypeEnum Type { get => PackageTypeEnum.Logout; }
        public TcpClient Client { get; set; }


        public void Execute()
        {
            Console.WriteLine("Unsupported command");
        }

        ICommand ICommandFactory.MakeCommand(TcpClient Client, DataPackage Cmd, PackageManager manager )
        {
            return new UnsupportedCommand() { Cmd = Cmd, Manager = manager ,Client = Client };
        }
    }
}
