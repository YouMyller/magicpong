using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;

namespace HD
{
    public class UDPInstance : MonoBehaviour
    {
        public static UDPInstance instance;

        public bool isServer;

        public IPAddress serverIp;

        List<IPEndPoint> clientList = new List<IPEndPoint>();

        public static string messageToDisplay;
        public Text text;

        UDPConnectedClient connection;

        private void Awake()
        {
            instance = this;

            if (serverIp == null)
            {
                this.isServer = true;
                connection = new UDPConnectedClient();
            }
            else
            {
                connection = new UDPConnectedClient(ip: serverIp);
                AddClient(new IPEndPoint(serverIp, Globals.port));
            }
        }

        private void Update()
        {
            text.text = messageToDisplay;
        }

        internal static void AddClient(IPEndPoint ipEndPoint)
        {
            if (instance.clientList.Contains(ipEndPoint) == false)
            {
                instance.clientList.Add(ipEndPoint);
            }
        }

        internal static void RemoveClient(IPEndPoint ipEndPoint)
        {
            instance.clientList.Remove(ipEndPoint);
        }

        private void OnApplicationQuit()
        {
            connection.Close();
        }

        public void Send(string message)
        {
            if (isServer)
            {
                messageToDisplay += message + Environment.NewLine;
            }

            BroadcastChatMessage(message);
        }

        internal static void BroadcastChatMessage(string message)
        {
            foreach (var ip in instance.clientList)
            {
                instance.connection.Send(message, ip);
            }
        }

        /*public void Listen()
        {
            UdpClient listener = new UdpClient();

            IPEndPoint serverEp = new IPEndPoint(IPAddress.Any, 123);

            while (true)
            {
                byte[] data = listener.Receive(ref serverEp);
                RaiseDataReceived(new ReceivedDataArgs(serverEp.Address, serverEp.Port, data));
            }
        }

        public delegate void DataReceived(object sender, ReceivedDataArgs args);

        public event DataReceived dataReceivedEvent;

        private void RaiseDataReceived(ReceivedDataArgs args)
        {
            if(dataReceivedEvent != null)
            {
                dataReceivedEvent(this, args);
            }
        }

        public class ReceivedDataArgs
        {
            public IPAddress ipAddress { get; set; }
            public int port { get; set; }
            public byte[] receivedBytes;

            public ReceivedDataArgs(IPAddress ip, int port, byte[] data)
            {
                this.ipAddress = ip;
                this.port = port;
                this.receivedBytes = data;
            }
        }*/

    }
}
