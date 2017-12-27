using System;
using System.Collections.Generic;
using System.Text;
using BlaBlaClient;
using Client.ICommand;
using Common;

namespace Client.ICommand
{
    public class MessageCommand : ICommand, ICommandFactory
    {

        public ClientCommandManager Manager { get; set; }
        public Command Cmd { get; set; }
        public PackageTypeEnum Type { get => PackageTypeEnum.Message; }

        public void Execute()
        {
            Message msg = Cmd.Content as Message;
            Console.WriteLine(msg.Sender.NickName + "# Wrote: " + msg.Text);
            Manager.MessageReceived?.Invoke(msg);
        }

        ICommand ICommandFactory.MakeCommand(Command Cmd, ClientCommandManager manager)
            =>  new MessageCommand() { Cmd = Cmd, Manager = manager };
        
    }
}
