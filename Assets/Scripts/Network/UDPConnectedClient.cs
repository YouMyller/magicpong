using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

namespace HD
{
    public class UDPConnectedClient : MonoBehaviour
    {
        readonly UdpClient connection;

        public UDPConnectedClient(IPAddress ip = null)
        {
            if (UDPInstance.instance.isServer)
            {
                connection = new UdpClient(Globals.port);
            }
            else
            {
                connection = new UdpClient();
            }

            connection.BeginReceive(OnReceive, null);
        }

        public void Close()
        {
            connection.Close();
        }

        void OnReceive(IAsyncResult ar)
        {
            try
            {
                IPEndPoint ipEndPoint = null;
                byte[] data = connection.EndReceive(ar, ref ipEndPoint);

                UDPInstance.AddClient(ipEndPoint);

                string message = System.Text.Encoding.UTF8.GetString(data);
                UDPInstance.messageToDisplay += message + Environment.NewLine;

                if (UDPInstance.instance.isServer)
                {
                    UDPInstance.BroadcastChatMessage(message);
                }
            }
            catch (SocketException e)
            {
                //kakka
            }

            connection.BeginReceive(OnReceive, null);
        }

        internal void Send(string message, IPEndPoint iPEndPoint)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
            connection.Send(data, data.Length, iPEndPoint);
        }
    }
}
