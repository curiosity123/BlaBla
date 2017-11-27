using Common;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace BlaBlaClient
{
    public interface IClientCommandManager
    {
        void CommandProcessor(TcpClient client, Command cmd);
        void RegisterNewUser(User user);
        void Login(User user);
        void Logout();
        void GetUsers();
        void GetConversation(User user);
        void Message(string text, List<User> users);
    }
}
