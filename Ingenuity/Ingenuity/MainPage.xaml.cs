using System;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;
using Windows.Devices.Spi;
using Windows.Devices.Enumeration;

namespace Ingenuity
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class MainPage : Page
    {
        public static SpiDevice Arduino;
        public MainPage()
        {
            this.InitializeComponent();
            InitSPI();
            NetworkCmd.NetworkInit();
        }

        private async void InitSPI()
        {
            try
            {
                var settings = new SpiConnectionSettings(0);    /* Create SPI initialization settings                               */
                settings.ClockFrequency = 5000000;              /* Datasheet specifies maximum SPI clock frequency of 10MHz         */
                settings.Mode = SpiMode.Mode0;

                string aqs = SpiDevice.GetDeviceSelector();                     /* Get a selector string that will return all SPI controllers on the system */
                var dis = await DeviceInformation.FindAllAsync(aqs);            /* Find the SPI bus controller devices with our selector string             */
                Arduino = await SpiDevice.FromIdAsync(dis[0].Id, settings);     /* Create an SpiDevice with our bus controller and SPI settings             */
            }
            /* If initialization fails, display the exception and stop running */
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Debug.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }
                return;
            }
            Debug.WriteLine("Status: Initialized");
        }

        public static void SendCommand(byte command)
        {
            byte[] buffer = { command };
            if (Arduino != null)
                Arduino.Write(buffer);
            else
                Debug.WriteLine("Trying to send commands to Arduino while it is not initialized!");
        }
    }
}
