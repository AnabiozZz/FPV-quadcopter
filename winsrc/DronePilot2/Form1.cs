using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using XInputDotNetPure;

namespace DronePilot
{
    public partial class MainScreen : Form
    {
        int run = 0;
        public MainScreen()
        {
            InitializeComponent();
        }

        private void MainScreen_Load(object sender, EventArgs e)
        {

        }

        private void txt_ipaddr_Enter(object sender, EventArgs e)
        {
            if (txt_ipaddr.Text == "Drone IP")
                txt_ipaddr.Text = "";
        }

        private void txt_ipaddr_Leave(object sender, EventArgs e)
        {
            if (txt_ipaddr.Text == "")
                txt_ipaddr.Text = "Drone IP";
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            {
                if (run == 1)
                {
                    run = 0;
                    btn_connect.Text = "Connect to drone";
                }
                else
                {
                    run = 1;
                    btn_connect.Text = "Disconnect";
                }
                    //create a new server
                var server = new UdpListener();

                //start listening for messages and copy the messages back to the client
                Task.Factory.StartNew(async () =>
                {
                    while (run == 1)
                    {
                        var received = await server.Receive();
                    }
                });

                //create a new client
                var client = UdpUser.ConnectTo(txt_ipaddr.Text, Int32.Parse(txt_ipport.Text));

                //wait for reply messages from server and send them to console 
                Task.Factory.StartNew(async () =>
                {
                    while (true)
                    {
                        var received = await client.Receive();
                        rtxt_log.AppendText(Convert.ToString(received.Message));
                        if (received.Message[1] == 0xFF)
                            break;
                    }
                });
                if (run == 1)
                    client.Send(new byte[2] { 0x00, 0x01 });
                else
                    client.Send(new byte[2] { 0x01, 0xFF });



                XInputDotNetPure.ButtonState prstart = 0, prback = 0, prleftStick = 0, prrightStick = 0, prleftShoulder = 0, prrightShoulder = 0, prguide = 0, pra = 0, prb = 0, prx = 0, pry = 0;
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
                        client.Send(new byte[2] { 0x01, 0xFF });                                        //TODO: need async thread
                        client.Send(new byte[2] { 0x07, (byte)state.Buttons.Guide });
                        break;
                    }

                    if (state.Buttons.X != prx)
                    {
                        client.Send(new byte[2] { 0x08, (byte)state.Buttons.X });
                    }

                    if (state.Buttons.Y != pry)
                    {
                        client.Send(new byte[2] { 0x09, (byte)state.Buttons.Y });
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
                Thread.Sleep(16);
            }
            }
        }
    }
}
