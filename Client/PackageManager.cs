using Common;
using Common.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using Client;
using Client.Commands;
using Common.ICommandPattern;

namespace BlaBlaClient
{
    public class PackageManager 
    {
        public ClientSettings Settings;
        public ClientCommunication Communication;
        public List<Conversation> Conversations;


        public Action<Message> MessageReceived { get; set; }
        public Action<List<User>> UsersListReceived { get; set; }
        public Action<bool> RegistrationResultReceived { get; set; }
        public Action<List<Conversation>> ConversationReceived { get; set; }
        public Action LogoutPackageReceived { get; set; }
        public Action<bool> LoginReceived { get; set; }



        private static IEnumerable<ICommandFactory> GetAvailableCommands()
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
        CommandParser PackageReceivedParser;


        public PackageManager(ClientSettings data, ClientCommunication communication, List<Conversation> conversations)
        {
            this.Communication = communication;
            this.Communication.PackageReceived += PackageProcessor;
            this.Settings = data;
            this.Conversations = conversations;
            PackageReceivedParser = new CommandParser(GetAvailableCommands());
        }

        public void PackageProcessor(TcpClient client, DataPackage cmd)
            => PackageReceivedParser.ParseCommand(cmd, this)?.Execute();
        


        public void RegisterNewUser(User user)
        {
            DataPackage cmd = new DataPackage() { Type = PackageTypeEnum.Register, Content = user };
            Communication.Send(cmd);
        }

        public void Login(User user)
        {
            DataPackage cmd = new DataPackage() { Type = PackageTypeEnum.Login, Content = user };
            Communication.Send(cmd);
        }

        public void Logout()
        {
            DataPackage cmd = new DataPackage() { Type = PackageTypeEnum.Logout, Content = Settings.CurrentUser };
            Communication.Send(cmd);
        }

        public void GetUsers()
        {
            DataPackage cmd = new DataPackage() { Type = PackageTypeEnum.Users, Content = Settings.CurrentUser };
            Communication.Send(cmd);
        }

        public void GetConversation(User user)
        {
            DataPackage cmd = new DataPackage() { Type = PackageTypeEnum.Conversation, Content = new Conversation() { Receiver = user, Sender = Settings.CurrentUser } };
            Communication.Send(cmd);
        }

        public void Message(string text, List<User> users)
        {
            Message msg = new Common.Message() { Sender = Settings.CurrentUser, UserList = users, Text = text };
            DataPackage cmd = new DataPackage() { Type = PackageTypeEnum.Message, Content = msg };
            Communication.Send(cmd);
        }
    }
}
