using System;
using System.Collections.Generic;
using System.Text;
using BlaBlaClient;
using Common;
using Common.ICommandPattern;

namespace Client.Commands
{
    public class RegisterCommand : ICommand, ICommandFactory
    {

        public PackageManager Manager { get; set; }
        public DataPackage Cmd { get; set; }
        public PackageTypeEnum Type { get => PackageTypeEnum.Register; }



        public void Execute()
        {
            Manager.RegistrationReceived?.Invoke(Cmd.Content as User != null);
            User u = Cmd.Content as User;

            if (u != null)
                Console.WriteLine("User " + u.NickName + " was registered succesfully! :)");
            else
                Console.WriteLine("User " + u.NickName + " was not registered, Try different nickname... :(");
        }

        ICommand ICommandFactory.MakeCommand(DataPackage Cmd, PackageManager manager)
            => new RegisterCommand() { Cmd = Cmd, Manager = manager };

    }
}
