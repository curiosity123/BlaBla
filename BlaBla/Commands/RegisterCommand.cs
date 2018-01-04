using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using BlaBlaServer;
using Common;
using Common.ICommandPattern;

namespace BlaBlaServer.Commands
{
    public class RegisterCommand : ICommand, ICommandFactory
    {
        public DataPackage Cmd { get; set; }
        public PackageManager Manager { get; set; }
        public PackageTypeEnum Type { get => PackageTypeEnum.Register; }
        public TcpClient Client { get; set; }


        public void Execute()
        {
            if (Cmd.Content is User && (from x in Manager.Settings.Users where x.NickName == (Cmd.Content as User).NickName select x).Count() == 0)
            {
                User usr = new User()
                {
                    NickName = (Cmd.Content as User).NickName,
                    Password = (Cmd.Content as User).Password,
                    Id = Guid.NewGuid()
                };
                Manager.Settings.Users.Add(usr);
                Manager.Communication.Send(Client, new DataPackage() { Type = PackageTypeEnum.Register, Content = usr });
            }
            else
                Manager.Communication.Send(Client, new DataPackage() { Type = PackageTypeEnum.Register, Content = null });
        }

        ICommand ICommandFactory.MakeCommand(TcpClient Client, DataPackage Cmd, PackageManager manager )
        {
            return new RegisterCommand() { Cmd = Cmd, Manager = manager ,Client = Client };
        }
    }
}
