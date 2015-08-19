using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Timers;
using System.Net;
using System.Windows.Threading;
using System.Net.Sockets;
using System.Diagnostics;
using Vlc.DotNet.Core.Interops;
using Vlc.DotNet.Core;
using Vlc.DotNet.Wpf;
using System.Reflection;
using System.IO;

namespace Ingenuity_Client
{
    /// <summary>
    /// Interaction logic for RobotControlWindow.xaml
    /// </summary>
    public partial class RobotControlWindow : Window
    {
        byte command;
        string robotAddress;
        string phoneAddress;
        Socket senderSock;
        public RobotControlWindow(string RobotIP, string PhoneIP)
        {
            InitializeComponent();
            robotAddress = RobotIP;
            phoneAddress = PhoneIP;

            if (phoneAddress != "")
            {
                myControl.MediaPlayer.VlcLibDirectoryNeeded += OnVlcControlNeedsLibDirectory;
            }

        }

        private void OnVlcControlNeedsLibDirectory(object sender, Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs e)
        {
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            if (currentDirectory == null)
                return;
            if (AssemblyName.GetAssemblyName(currentAssembly.Location).ProcessorArchitecture == ProcessorArchitecture.X86)
                e.VlcLibDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, @"..\..\lib\x86\"));
            else
                e.VlcLibDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, @"..\..\lib\x64\"));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SocketPermission perm = new SocketPermission(NetworkAccess.Connect, TransportType.Tcp, robotAddress, 8027);
            perm.Demand();
            IPAddress ipAddr = IPAddress.Parse(robotAddress);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 8027);

            senderSock = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            senderSock.NoDelay = false;
            senderSock.Connect(ipEndPoint);
            statusTextBlock.Text = "Spojeno s " + senderSock.RemoteEndPoint.ToString() + ".";

            DispatcherTimer dt = new DispatcherTimer();
            dt.Tick += new EventHandler(dt_Tick);
            dt.Interval = new TimeSpan(0, 0, 1);
            dt.Start();

            /*if (phoneAddress != "")
            {
                myControl.MediaPlayer.Play(new Uri("http://" + phoneAddress + ":8080/mjpeg/"));
            }*/
        }

        private void dt_Tick(object sender, EventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.W))
            {
                command |= 128; // 128 = 0b10000000
                wButton.IsChecked = true; // !!!
            }
            else
            {
                wButton.IsChecked = false;
            }

            if (Keyboard.IsKeyDown(Key.S))
            {
                command |= 64;  //  64 = 0b01000000
                sButton.IsChecked = true;
            }
            else
            {
                sButton.IsChecked = false;
            }

            if (Keyboard.IsKeyDown(Key.D))
            {
                command |= 32;  //  32 = 0b00100000
                dButton.IsChecked = true;
            }
            else
            {
                dButton.IsChecked = false;
            }

            if (Keyboard.IsKeyDown(Key.A))
            {
                command |= 16;  //  16 = 0b00010000
                aButton.IsChecked = true;
            }
            else
            {
                aButton.IsChecked = false;
            }

            byte[] buf = new byte[1];
            buf[0] = command;
            senderSock.Send(buf);

            Debug.WriteLine(Convert.ToString(command, 2));
            command = 0;
        }
    }
}
