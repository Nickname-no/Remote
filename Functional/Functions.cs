using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace Remote.Functional
{
    public static class Functions
    {
        public static void Sleep()
        {
            Application.SetSuspendState(PowerState.Suspend, true, true);
        }
        public static void Shutdown()
        {
            System.Diagnostics.Process.Start("Shutdown", "-s -t 10");
        }
        public static Bitmap SaveScreenshot()
        {
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            // Создание графического bitmap-объекта
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            // Берем скриншот из Take the screenshot от верхнего левого до нижнего правого угла
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            return bmpScreenshot;
        }

        public static void sendData(byte[] data, NetworkStream stream)
        {
            int bufferSize = 1024;
            byte[] dataLength = BitConverter.GetBytes(data.Length);
            stream.Write(dataLength, 0, 4);
            int bytesSent = 0;
            int bytesLeft = data.Length;
            while (bytesLeft > 0)
            {
                int curDataSize = Math.Min(bufferSize, bytesLeft);
                stream.Write(data, bytesSent, curDataSize);
                bytesSent += curDataSize;
                bytesLeft -= curDataSize;
            }
        }

        public static void sendString(byte[] data, NetworkStream stream)
        {

        }

        public static String sendClipBoard()
        {
            if (System.Windows.Clipboard.ContainsText())
            {
                return System.Windows.Clipboard.GetText();
            }
            return "";
        }
    }
}
