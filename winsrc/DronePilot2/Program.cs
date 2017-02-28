using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;


namespace DronePilot
{
    public struct Received
    {
        public IPEndPoint Sender;
        public byte[] Message;
    }

    abstract class UdpBase
    {
        protected UdpClient Client;

        protected UdpBase()
        {
            Client = new UdpClient();
        }

        public async Task<Received> Receive()
        {
            var result = await Client.ReceiveAsync();
            return new Received()
            {
                Message = result.Buffer,
                Sender = result.RemoteEndPoint
            };
        }
    }

    //Server
    class UdpListener : UdpBase
    {
        private IPEndPoint _listenOn;

        public UdpListener()
            : this(new IPEndPoint(IPAddress.Any, 32123))
        {
        }

        public UdpListener(IPEndPoint endpoint)
        {
            _listenOn = endpoint;
            Client = new UdpClient(_listenOn);
        }

        public void Reply(byte[] message, IPEndPoint endpoint)
        {
            Client.Send(message, message.Length, endpoint);
        }

    }

    //Client
    class UdpUser : UdpBase
    {
        private UdpUser() { }

        public static UdpUser ConnectTo(string hostname, int port)
        {
            var connection = new UdpUser();
            connection.Client.Connect(hostname, port);
            return connection;
        }

        public void Send(byte[] message)
        {
            Client.Send(message, message.Length);
        }

    }
    static class Program
    {

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainScreen());
        }
    }
}
