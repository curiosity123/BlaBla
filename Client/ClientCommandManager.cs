using Common;
using Common.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using Client;
using Client.ICommand;

namespace BlaBlaClient
{



    public class ClientCommandManager : IClientCommandManager
    {
        public ClientSettings Settings;
        public IClientCommunication communication;
        public List<Conversation> Conversations;
        public Action<Message> MessageReceived { get; set; }
        public Action<List<User>> UsersListReceived { get; set; }
        public Action<bool> RegistrationResultReceived { get; set; }
        public Action<List<Conversation>> ConversationReceived { get; set; }
        public Action LogoutPackageReceived { get; set; }
        public Action<bool> LoginReceived { get; set; }





        private static IEnumerable<ICommandFactory> GetAvailableReceivedCommands()
        {
            return new ICommandFactory[]
                {
                    new LoginCommand(),
                    new LogoutCommand(),
                    new UsersCommand(),
                    new MessageCommand(),
                    new RegisterCommand(),
                    new ConversationCommand(),
                    new UnsupportedCommand()
                };
        }

        CommandParser parser = new CommandParser(GetAvailableReceivedCommands());

        public ClientCommandManager(ClientSettings data, IClientCommunication communication, List<Conversation> conversations)
        {
            this.communication = communication;
            this.communication.PackageReceived += CommandProcessor;
            this.Settings = data;
            this.Conversations = conversations;
        }

        public void CommandProcessor(TcpClient client, Command cmd)
        {
            parser.ParseCommand(cmd, this)?.Execute();
        }


        public void RegisterNewUser(User user)
        {
            Command cmd = new Command() { Type = PackageTypeEnum.Register, Content = user };
            communication.Send(cmd);
        }

        public void Login(User user)
        {
            Command cmd = new Command() { Type = PackageTypeEnum.Login, Content = user };
            communication.Send(cmd);
        }

        public void Logout()
        {
            Command cmd = new Command() { Type = PackageTypeEnum.Logout, Content = Settings.CurrentUser };
            communication.Send(cmd);
        }

        public void GetUsers()
        {
            Command cmd = new Command() { Type = PackageTypeEnum.Users, Content = Settings.CurrentUser };
            communication.Send(cmd);
        }

        public void GetConversation(User user)
        {
            Command cmd = new Command() { Type = PackageTypeEnum.Conversation, Content = new Conversation() { Receiver = user, Sender = Settings.CurrentUser } };
            communication.Send(cmd);
        }

        public void Message(string text, List<User> users)
        {
            Message msg = new Common.Message() { Sender = Settings.CurrentUser, UserList = users, Text = text };
            Command cmd = new Command() { Type = PackageTypeEnum.Message, Content = msg };
            communication.Send(cmd);
        }
    }
}
