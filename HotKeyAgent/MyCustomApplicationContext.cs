using System;
using System.Windows.Forms;

using HotKeyAgent.Properties;
using UdpLib;

namespace HotKeyAgent
{
    internal class MyCustomApplicationContext : ApplicationContext
    {
        private NotifyIcon trayIcon;
    //    MenuItem item;
        private UDPSocket socket;
        public MyCustomApplicationContext()
        {

            socket = new UDPSocket();
            socket.Server("127.0.0.1",27000);

            socket.MessageRecived += Handle;

            //item = new MenuItem("Counter", Counter);
            //item.Enabled = false;

            // Initialize Tray Icon
            trayIcon = new NotifyIcon()
            {
                Icon = Resources.fav,
                ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("Exit", Exit)
                    //, item
                }),
                Visible = true
            };
        }

        private void Handle(object sender, UdpMessageEventArg e)
        {
            // this.item.Text = $"Message {e.Message}";

            try
            { // activate virtual desktop rc

                int.TryParse(e.Message, out int iParam);

                if (iParam>VirtualDesktop.Desktop.Count || iParam<0) return;

                switch (iParam)
                {
                    case 1: this.trayIcon.Icon = Resources._1; break;
                    case 2: this.trayIcon.Icon = Resources._2; break;
                    case 3: this.trayIcon.Icon = Resources._3; break;
                    case 4: this.trayIcon.Icon = Resources._4; break;
                    case 5: this.trayIcon.Icon = Resources._5; break;
                    default: this.trayIcon.Icon = Resources.fav; break;

                }

                iParam--;

                

                VirtualDesktop.Desktop.FromIndex(iParam).MakeVisible();
                //if (verbose) Console.WriteLine(", desktop '" + VirtualDesktop.Desktop.DesktopNameFromIndex(rc) + "' is active now");
            }
            catch(Exception x)
            { 
               //  this.item.Text = $"Message {e.Message} {x.Message}";
            }
        }

        void Counter(object sender, EventArgs e)
        {
        }


        void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            trayIcon.Visible = false;

            Application.Exit();
        }
    }
}