using HotKeyAgent.Properties;

using Microsoft.Win32;

using System;
using System.Linq;
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

            MenuItem mouseMenuItem = MouseMenu();

            MenuItem desktopMenuItem = new MenuItem("Desktops");
            for (int i = 0; i < 5; i++)
            {
                MenuItem item = new MenuItem($"Desktop {(i + 1)}", OpenDesktop);
                item.Tag = i;
                desktopMenuItem.MenuItems.Add(item);
            }

            trayIcon.ContextMenu.MenuItems.Add(mouseMenuItem);
            trayIcon.ContextMenu.MenuItems.Add(desktopMenuItem);
            trayIcon.ContextMenu.MenuItems.Add(new MenuItem("Exit", Exit));

        }

        private MenuItem MouseMenu()
        {
            string key = @"Computer\HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\HID";
            RegistryKey a = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Enum\HID");

            MenuItem moueseMenuItem = new MenuItem("Mouse");

            foreach (string name in a.GetSubKeyNames())
            {
                try
                {
                    RegistryKey b2 = a.OpenSubKey(name);
                    //RegistryKey b = Registry.LocalMachine.OpenSubKey($@"SYSTEM\CurrentControlSet\Enum\HID\${name}");


                    foreach (string subname in b2.GetSubKeyNames())
                    {

                        try
                        {
                            RegistryKey c = b2.OpenSubKey(subname);
                            RegistryKey d = c.OpenSubKey("Device Parameters");
                            string[] list = d.GetValueNames();

                            if (list.Any(s => s.Equals("FlipFlopHScroll")))
                            {
                                var val = d.GetValue("FlipFlopHScroll");
                                int flip = (int)val;

                                moueseMenuItem.MenuItems.Add(new MenuItem($"{name} ({(flip == 1 ? "good" : "not good")})", FlipFlop) { Tag = new MouseRegistry() { Path = d.Name, HFlip = flip } });
                            }

                            //Registry.LocalMachine.OpenSubKey($@"SYSTEM\CurrentControlSet\Enum\HID\{name}\{subname}\Device Parameters");
                        }
                        catch (Exception e)
                        {
                            ;
                        }

                    }



                }
                catch (Exception e)
                {
                    ;
                }

            }

            return moueseMenuItem;
            //for (int i = 0; i < 5; i++)
            //{
            //    MenuItem item = new MenuItem($"Desktop {(i + 1)}", OpenDesktop);
            //    item.Tag = i;
            //    moueseMenuItem.MenuItems.Add(item);
            //}
        }


        class MouseRegistry
        {
            public string Path { get; set; }
            public int HFlip { get; set; }
        }
        private void FlipFlop(object sender, EventArgs e)
        {
            try
            {
                var menu = (MenuItem)sender;
                var reg = (MouseRegistry)menu.Tag;
                Registry.SetValue(reg.Path, "FlipFlopHScroll", 1 - reg.HFlip);
                MessageBox.Show("done :). need reboot :(");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

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