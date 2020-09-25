using System;
using System.IO;
using UdpLib;

namespace HotKey
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            if (args == null || args.Length != 1) return;


            try
            {
                UDPSocket c = new UDPSocket();
                c.Client("127.0.0.1", 27000);
                c.Send(args[0]);
                File.AppendAllText("log.txt", $"{DateTime.Now} {args[0]}\n");
            }
            catch (Exception e)
            {
                File.AppendAllText("log.txt", $"{DateTime.Now} {e}\n");
                throw;
            }


        }
    }
}
