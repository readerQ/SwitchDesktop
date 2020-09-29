using HotKeyAgent.Properties;

using System;
using System.Windows.Forms;

using UdpLib;

namespace HotKeyAgent
{
    internal class MyCustomApplicationContext : ApplicationContext
    {

        private readonly NotifyIcon trayIcon;
        private readonly UDPSocket socket;

        public MyCustomApplicationContext()
        {

            socket = new UDPSocket();
            socket.Server("127.0.0.1", 27000); // hadrcoded

            socket.MessageRecived += Handle;

            trayIcon = new NotifyIcon()
            {
                Icon = Resources.random_icon_from_internet,
                ContextMenu = new ContextMenu(new MenuItem[] { }),
                Visible = true
            };

            for (int i = 0; i < 5; i++)
            {
                MenuItem item = new MenuItem($"Desktop {(i + 1)}", OpenDesktop);
                item.Tag = i;
                trayIcon.ContextMenu.MenuItems.Add(item);
            }

            trayIcon.ContextMenu.MenuItems.Add(new MenuItem("Exit", Exit));

        }

        private void OpenDesktop(object sender, EventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            int tag = (int)menuItem.Tag;
            ActivateDesktop(tag + 1);
        }

        private void Handle(object sender, UdpMessageEventArg e)
        {
            int.TryParse(e.Message, out int iParam);
            ActivateDesktop(iParam);

        }

        private void ActivateDesktop(int iParam)
        {
            try
            {

                if (iParam > VirtualDesktop.Desktop.Count || iParam < 0)
                {
                    return;
                }

                switch (iParam)
                {
                    case 1: trayIcon.Icon = Resources._1; break;
                    case 2: trayIcon.Icon = Resources._2; break;
                    case 3: trayIcon.Icon = Resources._3; break;
                    case 4: trayIcon.Icon = Resources._4; break;
                    case 5: trayIcon.Icon = Resources._5; break;
                    default: trayIcon.Icon = Resources.random_icon_from_internet; break;

                }

                iParam--;

                VirtualDesktop.Desktop.FromIndex(iParam).MakeVisible();
            }
            catch
            {
                // understand & forgive 
                // forgive & forgot
            }

        }

        private void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            trayIcon.Visible = false;

            Application.Exit();
        }
    }
}