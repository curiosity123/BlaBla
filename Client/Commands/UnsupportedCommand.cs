using System;
using System.Collections.Generic;
using System.Text;
using BlaBlaClient;
using Common;
using Common.ICommandPattern;

namespace Client.Commands
{
    public class UnsupportedCommand : ICommand, ICommandFactory
    {


        public PackageManager Manager { get; set; }
        public DataPackage Cmd { get; set; }
        public PackageTypeEnum Type { get => PackageTypeEnum.Unsupported; }



        public void Execute()
        {
        }

        ICommand ICommandFactory.MakeCommand(DataPackage Cmd, PackageManager manager)
               => new UnsupportedCommand() { Cmd = Cmd, Manager = manager };

    }
}
