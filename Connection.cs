using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Remote.Functional;

namespace Remote
{
    class Connection
    {

        public static TcpClient client { get; private set;}
        private static TcpListener listener;
        private static string ipString;
        public static IPEndPoint ep { get; private set;}

        public Connection()
        {
            IpAddressRessult();
        }
        public void IpAddressRessult()
        {
            IPAddress[] localIp = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress address in localIp)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipString = address.ToString();
                }
            }
            ep = new IPEndPoint(IPAddress.Parse(ipString), 1234);
            listener = new TcpListener(ep);
        }

        public void ConnectionToClient()
        {
            
            listener.Start();
            client = listener.AcceptTcpClient();
            while (client.Connected)
            {
                try
                {
                    const int bytesize = 1024 * 1024;
                    byte[] buffer = new byte[bytesize];
                    string x = client.GetStream().Read(buffer, 0, bytesize).ToString();
                    var data = ASCIIEncoding.ASCII.GetString(buffer);
                    if (data.ToUpper().Contains("SLP2"))
                    {
                        Functions.Sleep();
                    }
                    else if (data.ToUpper().Contains("SHTD3"))
                    {
                        Console.WriteLine("Pc is going to Shutdown!" + " \n");
                        Functions.Shutdown();
                    }
                    else if (data.ToUpper().Contains("TSC1"))
                    {
                        Console.WriteLine("Take Screenshot!" + " \n");
                        var bitmap = Functions.SaveScreenshot();
                        var stream = new MemoryStream();
                        bitmap.Save(stream, ImageFormat.Bmp);
                        Functions.sendData(stream.ToArray(), client.GetStream());
                    }
                    else if (data.ToUpper().Contains("CBC1"))
                    {
                        var clipboard = Functions.sendClipBoard();
                        Functions.sendData(Encoding.UTF8.GetBytes(clipboard), client.GetStream());


                    }
                }
                catch (Exception exc)
                {
                    client.Dispose();
                    client.Close();
                }
            }
        }

        
    }
}
