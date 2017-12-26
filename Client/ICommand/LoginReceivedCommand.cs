using System;
using System.Collections.Generic;
using System.Text;
using BlaBlaClient;
using Common;

namespace Client.ICommand
{
    public class LoginReceivedCommand : ICommand, IClientCommandFactory
    {

        public ClientSettings settings { get; set; }
        public IClientCommunication communication { get; set; }
        public List<Conversation> conversationList { get; set; }
        public PackageTypeEnum Type
        {
            get { return PackageTypeEnum.Login; }
        }

        public Command Cmd { get; set; }


        public string CommandName => "LoginReceivedCommand";

        public string Description => "Login command received";


        public void Execute()
        {
            if (Cmd.Content is User)
            {
                settings.CurrentUser = Cmd.Content as User;
                communication.StartSendingAlivePackage(settings.CurrentUser);
                Console.WriteLine("You are logged in");
            }

        }

        ICommand IClientCommandFactory.MakeCommand(Command Cmd, ClientSettings Settings, IClientCommunication Communication, List<Conversation> ConversationList)
        {
            return new LoginReceivedCommand() { Cmd = Cmd, settings = Settings, conversationList = conversationList, communication = Communication };

        }
    }
}
