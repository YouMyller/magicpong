using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Server
{
    class Server
    {
        static void Main(string[] args)
        {
            EventBasedNetListener listener = new EventBasedNetListener();
            NetManager server = new NetManager(listener);

            server.Start(2310 /* port */);

            listener.ConnectionRequestEvent += request =>
            {
                if (server.PeersCount < 10 /* max connections */)
                    request.AcceptIfKey("SomeConnectionKey");
                else
                    request.Reject();
            };

            listener.PeerConnectedEvent += peer =>
            {
                Console.WriteLine("We got connection: {0}", peer.EndPoint);     // Show peer ip

                NetDataWriter writer = new NetDataWriter();                     // Create writer class            
                writer.Put(peer.Id.ToString());                                 // Put some string
                Console.WriteLine(peer.Id);
                peer.Send(writer, DeliveryMethod.ReliableOrdered);              // Send with reliability
                writer.Reset();
            };

            listener.PeerConnectedEvent -= peer =>
            {
                Console.WriteLine("We lost connection.");
            };

            listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod) =>
            {
                if (fromPeer.Id == 0)
                {
                    string tempS = dataReader.GetString();
                    float tempX = dataReader.GetFloat();
                    float tempY = dataReader.GetFloat();
                    float tempZ = dataReader.GetFloat();
                    Console.WriteLine("We got your spawn position: " + tempX + " " + tempY + " " + tempZ + " from peer: " + fromPeer.Id);

                    NetDataWriter writer = new NetDataWriter();

                    writer.Put(tempS);
                    writer.Put(tempX);
                    writer.Put(tempY);
                    writer.Put(tempZ);
                    server.SendToAll(writer, DeliveryMethod.ReliableOrdered);
                    writer.Reset();
                }
                else
                {
                    string ploo = dataReader.GetString();
                    Console.WriteLine("We got your message: " + ploo + " from peer: " + fromPeer.Id);

                    NetDataWriter writer = new NetDataWriter();
                    writer.Put(ploo);
                    server.SendToAll(writer, DeliveryMethod.ReliableOrdered);

                    //Console.WriteLine("We got: {0}", dataReader.GetString(100 /* max length of string */));
                    dataReader.Recycle();
                }
            };

            while (!Console.KeyAvailable)
            {
                server.PollEvents();
                Thread.Sleep(15);
            }
            server.Stop();
        }
    }
}
