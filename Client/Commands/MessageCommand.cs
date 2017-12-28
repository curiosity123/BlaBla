using System;
using System.Collections.Generic;
using System.Text;
using BlaBlaClient;

using Common;
using Common.ICommandPattern;

namespace Client.Commands
{
    public class MessageCommand : ICommand, ICommandFactory
    {

        public PackageManager Manager { get; set; }
        public DataPackage Cmd { get; set; }
        public PackageTypeEnum Type { get => PackageTypeEnum.Message; }

        public void Execute()
        {
            Message msg = Cmd.Content as Message;
            Console.WriteLine(msg.Sender.NickName + "# Wrote: " + msg.Text);
            Manager.MessageReceived?.Invoke(msg);
        }

        ICommand ICommandFactory.MakeCommand(DataPackage Cmd, PackageManager manager)
            =>  new MessageCommand() { Cmd = Cmd, Manager = manager };
        
    }
}
