using BlaBlaClient;
using Common;
using Common.ICommandPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client

{
    public interface ICommandFactory
    {
        DataPackage Cmd { get; set; }
        PackageManager Manager { get; set; }
        PackageTypeEnum Type { get; }


        ICommand MakeCommand(DataPackage Cmd, PackageManager manager);
    }
}
