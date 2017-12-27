using BlaBlaClient;
using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.ICommand

{
    public interface ICommandFactory
    {
        Command Cmd { get; set; }
        ClientCommandManager Manager { get; set; }
        PackageTypeEnum Type { get; }


        ICommand MakeCommand(Command Cmd, ClientCommandManager manager);
    }
}
