using Common;

namespace BlaBlaClient
{

    public class Client
    {
        Communication communication;
        public PackageManager packageManager;
        public Settings settings = new Settings();


        private Client(ISerialization serialization, string ip, int port)
        {
            communication = new Communication(serialization, ip, port);
            packageManager = new PackageManager(settings, communication);
        }

        private Client() { }

        public static Client Create(ISerialization serialization, string ip, int port)
        {
            Client client = new Client(serialization, ip, port);
            return client;
        }


        public void Run()
        {
            communication.Connect();
        }
        public void Stop()
        {
            communication.Disconnect();
        }
    }
}
