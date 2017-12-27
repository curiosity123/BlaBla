using System;
using System.Collections.Generic;
using System.Text;
using BlaBlaClient;
using Client.ICommand;
using Common;

namespace Client.ICommand
{
    public class RegisterCommand : ICommand, ICommandFactory
    {

        public ClientCommandManager Manager { get; set; }
        public Command Cmd { get; set; }
        public PackageTypeEnum Type { get => PackageTypeEnum.Register; }



        public void Execute()
        {
            Manager.RegistrationResultReceived?.Invoke(Cmd.Content as User != null);
            User u = Cmd.Content as User;

            if (u != null)
                Console.WriteLine("User " + u.NickName + " was registered succesfully! :)");
            else
                Console.WriteLine("User " + u.NickName + " was not registered, Try different nickname... :(");
        }

        ICommand ICommandFactory.MakeCommand(Command Cmd, ClientCommandManager manager)
            => new RegisterCommand() { Cmd = Cmd, Manager = manager };

    }
}
