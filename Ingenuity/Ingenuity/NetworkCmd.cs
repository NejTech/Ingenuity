using System;
using Windows.Foundation;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using System.Diagnostics;

namespace Ingenuity
{
    public static class NetworkCmd
    {
        // if no host, be client, otherwise be a host
        private static String hostName = "";
        private const String hostPort = "8027";

        public static void NetworkInit()
        {
            Debug.WriteLine("NetworkInit() port={0}", hostPort);
            if (listener == null) StartListener();
        }

        public static long msLastSendTime;

        #region ----- host connection ----
        static StreamSocketListener listener;
        public static async void StartListener()
        {
            try
            {
                listener = new StreamSocketListener();
                listener.ConnectionReceived += OnConnection;
                await listener.BindServiceNameAsync(hostPort);
                Debug.WriteLine("Listening on {0}", hostPort);
            }
            catch (Exception e)
            {
                Debug.WriteLine("StartListener() - Unable to bind listener. " + e.Message);
            }
        }

        static async void OnConnection(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            try
            {
                DataReader reader = new DataReader(args.Socket.InputStream);
                while (true)
                {
                    uint len = await reader.LoadAsync(1);
                    if (len > 0)
                    {
                        byte b = reader.ReadByte();
                        Debug.WriteLine("Network Received: '{0}'", Convert.ToString(b,2));
                        MainPage.SendCommand(b);
                        //break;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("OnConnection() - " + e.Message);
            }
        }
        #endregion
    }
}
