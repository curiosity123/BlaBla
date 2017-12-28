using System;
using System.Collections.Generic;
using System.Text;
using BlaBlaClient;
using Common;
using Common.ICommandPattern;

namespace Client.Commands
{
    public class ConversationCommand : ICommand, ICommandFactory
    {

        public PackageManager Manager { get; set; }
        public DataPackage Cmd { get; set; }
        public PackageTypeEnum Type { get => PackageTypeEnum.Conversation; }


        public void Execute()
        {
            Manager.ConversationReceived?.Invoke(Cmd.Content as List<Conversation>);
            Manager.Conversations = Cmd.Content as List<Conversation>;
            foreach (Conversation c in Manager.Conversations)
                Console.WriteLine(c.Sender.NickName + "Wrote: " + c.Sentence.Text);
        }

        ICommand ICommandFactory.MakeCommand(DataPackage Cmd, PackageManager manager)
            =>  new ConversationCommand() { Cmd = Cmd, Manager = manager };
        
    }
}
