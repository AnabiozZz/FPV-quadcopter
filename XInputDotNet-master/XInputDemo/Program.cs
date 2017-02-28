using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using XInputDotNetPure;

namespace XInputDemo
{

    public struct Received
    {
        public IPEndPoint Sender;
        public byte [] Message;
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





    class Program
    {
        static void Main(string[] args)
        {

            //create a new server
            var server = new UdpListener();

            //start listening for messages and copy the messages back to the client
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    var received = await server.Receive();
                    if (received.Message[1] == 0xFF)
                        break;
                }
            });

            //create a new client
            var client = UdpUser.ConnectTo("192.168.1.2", 23455);

            //wait for reply messages from server and send them to console 
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    var received = await client.Receive();
                    Console.WriteLine(received.Message);
                    if (received.Message[1] == 0xFF)
                        break;
                }
            });

            //type ahead :-)
            string read;
            do
            {
                read = Console.ReadLine();

                switch (read)
                {
                    case "start":
                        client.Send(new byte[2] { 0x00, 0x01 });
                        break;
                    case "stop":
                        client.Send(new byte[2] { 0x00, 0xFF });
                        break;
                }

                
            } while (read != "quit");

            ButtonState prstart = 0, prback = 0, prleftStick = 0, prrightStick = 0, prleftShoulder = 0, prrightShoulder = 0, prguide = 0, pra = 0, prb = 0, prx = 0, pry = 0;
            float prrigthX = 0, prrigthY = 0, prleftX = 0, prleftY = 0;

            while (true)
            {
                GamePadState state = GamePad.GetState(PlayerIndex.One);

                if (state.Buttons.Start != prstart)
                {
                    client.Send(new byte[2] { 0x01, (byte)state.Buttons.Start });
                }

                if (state.Buttons.Back != prback)
                {
                    client.Send(new byte[2] { 0x02, (byte)state.Buttons.Back });
                }

                if (state.Buttons.LeftStick != prleftStick)
                {
                    client.Send(new byte[2] { 0x03, (byte)state.Buttons.LeftStick });
                }

                if (state.Buttons.RightStick != prrightStick)
                {
                    client.Send(new byte[2] { 0x04, (byte)state.Buttons.RightStick });
                }

                if (state.Buttons.LeftShoulder != prleftShoulder)
                {
                    client.Send(new byte[2] { 0x05, (byte)state.Buttons.LeftShoulder });
                }

                if (state.Buttons.RightShoulder != prrightShoulder)
                {
                    client.Send(new byte[2] { 0x06, (byte)state.Buttons.RightShoulder });
                }

                if (state.Buttons.Guide != prguide)
                {
                    client.Send(new byte[2] { 0x07, (byte)state.Buttons.Guide });
                }

                if (state.Buttons.X != prx)
                {
                    client.Send(new byte[2] { 0x08, (byte)state.Buttons.X });
                }

                if (state.Buttons.Y != pry)
                {
                    client.Send(new byte[2] { 0x09, (byte)state.Buttons.Y});
                }

                if (state.Buttons.A != pra)
                {
                    client.Send(new byte[2] { 0x0A, (byte)state.Buttons.A });
                }

                if (state.Buttons.B != prb)
                {
                    client.Send(new byte[2] { 0x0B, (byte)state.Buttons.B });
                }


                if (state.ThumbSticks.Left.X != prrigthX)
                {
                    client.Send(new byte[2] { 0x0C, (byte)((((state.ThumbSticks.Left.X + 1.0f) / 2.0f) * 255.0f) - 128.0f) });
                }
                if (state.ThumbSticks.Left.Y != prrigthY)
                {
                    client.Send(new byte[2] { 0x0D, (byte)((((state.ThumbSticks.Left.Y + 1.0f) / 2.0f) * 255.0f) - 128.0f) });
                }
                if (state.ThumbSticks.Right.X != prleftX)
                {
                    client.Send(new byte[2] { 0x0E, (byte)((((state.ThumbSticks.Right.X + 1.0f) / 2.0f) * 255.0f) - 128.0f) });
                }
                if (state.ThumbSticks.Right.Y != prleftY)
                {
                    client.Send(new byte[2] { 0x0F, (byte)((((state.ThumbSticks.Right.Y + 1.0f) / 2.0f) * 255.0f) - 128.0f) });
                }

                

                /*Console.WriteLine("IsConnected {0} Packet #{1}", state.IsConnected, state.PacketNumber);
                Console.WriteLine("\tTriggers {0} {1}", state.Triggers.Left, state.Triggers.Right);
                Console.WriteLine("\tD-Pad {0} {1} {2} {3}", (byte)state.DPad.Up, state.DPad.Right, state.DPad.Down, state.DPad.Left);
                Console.WriteLine("\tButtons Start {0} Back {1} LeftStick {2} RightStick {3} LeftShoulder {4} RightShoulder {5} Guide {6} A {7} B {8} X {9} Y {10}",
                    state.Buttons.Start, state.Buttons.Back, state.Buttons.LeftStick, state.Buttons.RightStick, state.Buttons.LeftShoulder, state.Buttons.RightShoulder,
                    state.Buttons.Guide, state.Buttons.A, state.Buttons.B, state.Buttons.X, state.Buttons.Y);
                Console.WriteLine("\tSticks Left {0} {1} Right {2} {3}", state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y, state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y);
                GamePad.SetVibration(PlayerIndex.One, state.Triggers.Left, state.Triggers.Right);
                Thread.Sleep(16);*/
            }
        }
    }
}
