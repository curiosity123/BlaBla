using Common;
using Common.ICommandPattern;
using System.Linq;
using System.Net.Sockets;

namespace BlaBlaServer.Commands
{
    public class LoginCommand : ICommand, ICommandFactory
    {
        public DataPackage Cmd { get; set; }
        public PackageManager Manager { get; set; }
        public PackageTypeEnum Type { get => PackageTypeEnum.Login; }
        public TcpClient Client { get; set; }


        public void Execute()
        {
            if (Cmd.Content is User)
            {
                if ((from x in Manager.Settings.Users where x.NickName == (Cmd.Content as User).NickName && x.Password == (Cmd.Content as User).Password select x).Count() > 0)
                {
                    var usr = (from x in Manager.Settings.Users where x.NickName == (Cmd.Content as User).NickName && x.Password == (Cmd.Content as User).Password select x).FirstOrDefault();
                    if (usr != null)
                    {
                        (from x in Manager.Settings.Sessions where x.Client == Client select x).First().User = usr;
                        Manager.Communication.Send(Client, new DataPackage() { Type = PackageTypeEnum.Login, Content = usr });
                    }
                }
                Manager.Communication.Send(Client, new DataPackage() { Type = PackageTypeEnum.Login, Content = null });
            }

        }

        ICommand ICommandFactory.MakeCommand(TcpClient Client, DataPackage Cmd, PackageManager manager )
        {
            return new LoginCommand() { Cmd = Cmd, Manager = manager ,Client = Client };
        }
    }
}
