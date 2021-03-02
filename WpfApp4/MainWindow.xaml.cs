using Grpc.Core;
using GrpcAuthorClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

namespace WpfApp4
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var serverAddress = "https://127.0.0.1:5001";
            //Channel channel = new Channel(serverAddress, ChannelCredentials.Insecure);
            //Channel channel = new Channel(serverAddress, ChannelCredentials.Insecure);
            Channel channel = new Channel("127.0.0.1",5000, ChannelCredentials.Insecure);
            var client = new Greeter.GreeterClient(channel);
            var replay = client.SayHello(new HelloRequest { Name = "Auto" ,Id=32});
            Debug.WriteLine($"{replay.Message}");
            Console.WriteLine($"{replay.Message}");
            //
            channel.ShutdownAsync().Wait();
            Console.ReadLine();

        }
        
    }
}
