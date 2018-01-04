using Common;
using Common.ICommandPattern;
using System.Linq;
using System.Net.Sockets;

namespace BlaBlaServer.Commands
{
    public class LogoutCommand : ICommand, ICommandFactory
    {
        public DataPackage Cmd { get; set; }
        public PackageManager Manager { get; set; }
        public PackageTypeEnum Type { get => PackageTypeEnum.Logout; }
        public TcpClient Client { get; set; }


        public void Execute()
        {
            var session = (from x in Manager.Settings.Sessions where x.User.Id == (Cmd.Content as User).Id select x);
            Manager.Communication.Send(Client, new DataPackage() { Type = PackageTypeEnum.Logout, Content = null });
        }

        ICommand ICommandFactory.MakeCommand(TcpClient Client, DataPackage Cmd, PackageManager manager )
        {
            return new LogoutCommand() { Cmd = Cmd, Manager = manager ,Client = Client };
        }
    }
}
