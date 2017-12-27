using System;
using System.Collections.Generic;
using System.Text;
using BlaBlaClient;
using Client.ICommand;
using Common;

namespace Client.ICommand
{
    public class ConversationCommand : ICommand, ICommandFactory
    {

        public ClientCommandManager Manager { get; set; }
        public Command Cmd { get; set; }
        public PackageTypeEnum Type { get => PackageTypeEnum.Conversation; }


        public void Execute()
        {
            Manager.ConversationReceived?.Invoke(Cmd.Content as List<Conversation>);
            Manager.Conversations = Cmd.Content as List<Conversation>;
            foreach (Conversation c in Manager.Conversations)
                Console.WriteLine(c.Sender.NickName + "Wrote: " + c.Sentence.Text);
        }

        ICommand ICommandFactory.MakeCommand(Command Cmd, ClientCommandManager manager)
            =>  new ConversationCommand() { Cmd = Cmd, Manager = manager };
        
    }
}
