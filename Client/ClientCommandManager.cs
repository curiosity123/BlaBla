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
        ClientSettings Settings;
        IClientCommunication communication;
        List<Conversation> conversations;

        public Action<Message> MessageReceived { get; set; }
        public Action<List<User>> UsersListReceived { get; set; }
        public Action<bool> RegistrationResultReceived { get; set; }
        public Action<List<Conversation>> ConversationReceived { get; set; }
        public Action LogoutPackageReceived { get; set; }
        public Action<bool> LoginReceived { get; set; }





        static IEnumerable<IClientCommandFactory> GetAvailableCommands()
        {
            return new IClientCommandFactory[]
                {
                    new LoginReceivedCommand()
                };
        }

        public ClientCommandManager(ClientSettings data, IClientCommunication communication, List<Conversation> conversations)
        {
            this.communication = communication;
            this.communication.PackageReceived += CommandProcessor;
            this.Settings = data;
            this.conversations = conversations;
        }

        public void CommandProcessor(TcpClient client, Command cmd)
        {
                

            CommandParser parser = new CommandParser(GetAvailableCommands());
            var command = parser.ParseCommand(cmd, Settings, communication, conversations);

            if (command != null)
                command.Execute();


            if (cmd.Type == PackageTypeEnum.Users)
            {
                Settings.ActiveUsers = cmd.Content as List<User>;
                UsersListReceived?.Invoke(cmd.Content as List<User>);
            }

            if (cmd.Type == PackageTypeEnum.Login && cmd.Content is User)
            {
                Settings.CurrentUser = cmd.Content as User;
                communication.StartSendingAlivePackage(Settings.CurrentUser);
                Console.WriteLine("You are logged in");
            }

            if (cmd.Type == PackageTypeEnum.Logout)
            {
                LogoutPackageReceived?.Invoke();
                communication.StopSendingAlivePackage();
                Settings.CurrentUser = new User();
                Console.WriteLine("You are logout");
            }

            if (cmd.Type == PackageTypeEnum.Message)
            {
                Message msg = cmd.Content as Message;
                Console.WriteLine(msg.Sender.NickName + "# Wrote: " + msg.Text);
                MessageReceived?.Invoke(msg);
            }

            if (cmd.Type == PackageTypeEnum.Conversation)
            {
                ConversationReceived?.Invoke(cmd.Content as List<Conversation>);

                conversations = cmd.Content as List<Conversation>;
                foreach (Conversation c in conversations)
                    Console.WriteLine(c.Sender.NickName + "Wrote: " + c.Sentence.Text);
            }

            if (cmd.Type == PackageTypeEnum.Register)
            {
                RegistrationResultReceived?.Invoke(cmd.Content as User != null);

                User u = cmd.Content as User;


                if (u != null)
                    Console.WriteLine("User " + u.NickName + " was registered succesfully! :)");
                else
                    Console.WriteLine("User " + u.NickName + " was not registered, Try different nickname... :(");
            }
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
