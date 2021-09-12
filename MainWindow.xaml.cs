using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;


namespace Remote
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Connection connection = new Connection();
            ipaddress.Text = Convert.ToString(Connection.ep.Address);
            port.Text = Convert.ToString(Connection.ep.Port);
            QRCodeEncoder encoder = new QRCodeEncoder();
            Bitmap qrcode = encoder.Encode(Convert.ToString(Connection.ep.Address) + "|" + Convert.ToString(Connection.ep.Port));
            qrcode.Save("C:\\Users\\Nick Petrenko\\source\\repos\\Remote\\qrcode.jpg");
            qrCode.Source = new BitmapImage(new Uri("C:\\Users\\Nick Petrenko\\source\\repos\\Remote\\qrcode.jpg"));
            var connect = new Thread(connection.ConnectionToClient);
            connect.Start();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
