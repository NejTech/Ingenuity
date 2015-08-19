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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ingenuity_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (RobotAddressTextBox.Text == "")
            {
                MessageBoxResult warn = MessageBox.Show("Pokud teď stisknete OK, spadne to :(","Varování",MessageBoxButton.OKCancel,MessageBoxImage.Warning);
                if (warn == MessageBoxResult.OK)
                {
                    RobotControlWindow w = new RobotControlWindow(RobotAddressTextBox.Text, CameraAddressTextBox.Text);
                    w.Show();
                    this.Close();
                }
            }
            else if (CameraAddressTextBox.Text == "")
            {
                MessageBoxResult warn = MessageBox.Show("Nebyla zadána IP adresa mobilu! Nebude fungovat video!", "Varování", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (warn == MessageBoxResult.OK)
                {
                    RobotControlWindow w = new RobotControlWindow(RobotAddressTextBox.Text, CameraAddressTextBox.Text);
                    w.Show();
                    this.Close();
                }
            }
            /*else
            {
                MessageBox.Show("Musíte zadat alespoň IP adresu robota!");
            }*/
            RobotControlWindow rcw = new RobotControlWindow(RobotAddressTextBox.Text, CameraAddressTextBox.Text);
            rcw.Show();
            this.Close();
        }
    }
}
