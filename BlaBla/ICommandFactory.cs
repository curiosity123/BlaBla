using BlaBlaServer;
using Common;
using Common.ICommandPattern;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace BlaBlaServer

{
    public interface ICommandFactory
    {
        DataPackage Cmd { get; set; }
        ServerPackageManager Manager { get; set; }
        PackageTypeEnum Type { get; }
        TcpClient Client { get; set; }


        ICommand MakeCommand(TcpClient Client ,DataPackage Cmd, ServerPackageManager manager);
    }
}
