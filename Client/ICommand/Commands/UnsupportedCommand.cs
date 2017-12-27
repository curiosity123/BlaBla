using System;
using System.Collections.Generic;
using System.Text;
using BlaBlaClient;
using Common;


namespace Client.ICommand
{
    public class UnsupportedCommand : ICommand, ICommandFactory
    {


        public ClientCommandManager Manager { get; set; }
        public Command Cmd { get; set; }
        public PackageTypeEnum Type { get => PackageTypeEnum.Unsupported; }



        public void Execute()
        {
        }

        ICommand ICommandFactory.MakeCommand(Command Cmd, ClientCommandManager manager)
               => new UnsupportedCommand() { Cmd = Cmd, Manager = manager };

    }
}
