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
        enum GameState { Start, Game, Win }
        //If Win, send victory/loss messages

        enum BallDir { Up, Down }

        static void Main(string[] args)
        {
            EventBasedNetListener listener = new EventBasedNetListener();
            NetManager server = new NetManager(listener);

            GameState state = GameState.Start;
            BallDir dir = BallDir.Up;

            //Player one position
            float pOnePosX = 0;
            float pOnePosY = 0;
            float pOnePosZ = 0;

            //Player two position
            float pTwoPosX = 0;
            float pTwoPosY = 0;
            float pTwoPosZ = 0;

            //Ball position
            float ballPosX = 0;
            float ballPosY = 0;
            float ballPosZ = 0;

            bool pOneColl = false;
            bool pTwoColl = false;
            bool ballColl = false;


            server.Start(2310 /* port */);                                      //2310 is school port

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
                writer.Put(peer.Id);                                 // Put some string
                Console.WriteLine("Peers id: " + peer.Id);
                peer.Send(writer, DeliveryMethod.ReliableOrdered);              // Send with reliability
                writer.Reset();
            };

            listener.PeerConnectedEvent -= peer =>
            {
                Console.WriteLine("We lost connection.");
            };

            listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod) =>
            {
                string input = dataReader.GetString();
                bool collision = dataReader.GetBool();

                if (input == "SET POS")
                {
                    if (fromPeer.Id == 0)
                    {
                        pOnePosX = dataReader.GetFloat();
                        pOnePosY = dataReader.GetFloat();
                        pOnePosZ = dataReader.GetFloat();
                        Console.WriteLine("We got player 1 start position: " + pOnePosX + " " + pOnePosY + " " + pOnePosZ + " from peer: " + fromPeer.Id);
                    }
                    else if (fromPeer.Id == 1)
                    {
                        pTwoPosX = dataReader.GetFloat();
                        pTwoPosY = dataReader.GetFloat();
                        pTwoPosZ = dataReader.GetFloat();
                        Console.WriteLine("We got player 2 start position: " + pTwoPosX + " " + pTwoPosY + " " + pTwoPosZ + " from peer: " + fromPeer.Id);
                    }
                }

                if (input == "A" || input == "D")
                {
                    NetDataWriter writer = new NetDataWriter();

                    if (fromPeer.Id == 0)
                    {
                        pOneColl = collision;

                        if (!pOneColl)
                        {
                            //Calculate new position:
                            if (input == "A")
                                pOnePosX -= .2f;
                            else if (input == "D")
                                pOnePosX += .2f;

                            writer.Put(fromPeer.Id);
                            writer.Put("MOVE");
                            writer.Put(pOnePosX);
                            writer.Put(pOnePosZ);

                            Console.WriteLine("Player 1 brand new pos: " + pOnePosX + " " + pOnePosY + " " + pOnePosZ + " from peer: " + fromPeer.Id);
                        }
                        else
                            Console.WriteLine("Peer " + fromPeer.Id + " is colliding");
                    }
                    else
                    {
                        pTwoColl = collision;

                        if (!pTwoColl)
                        {
                            //Calculate new position:
                            if (input == "A")
                                pTwoPosX += .2f;
                            else if (input == "D")
                                pTwoPosX -= .2f;

                            writer.Put(fromPeer.Id);
                            writer.Put("MOVE");
                            writer.Put(pTwoPosX);
                            writer.Put(pTwoPosZ);

                            Console.WriteLine("Player 2 brand new pos: " + pTwoPosX + " " + pTwoPosY + " " + pTwoPosZ + " from peer: " + fromPeer.Id);
                        }
                    }

                    server.SendToAll(writer, DeliveryMethod.ReliableOrdered);
                    writer.Reset();
                }

                if (input == "BALL MOVE")
                {
                    NetDataWriter writer = new NetDataWriter();
                    ballColl = collision;

                    ballPosX = dataReader.GetFloat();
                    ballPosY = dataReader.GetFloat();
                    ballPosZ = dataReader.GetFloat();

                    if (fromPeer.Id == 0)
                        ballPosZ += 0.9f;
                    else if (fromPeer.Id == 1)
                        ballPosZ -= 0.9f;
                    
                    if (ballColl)
                    {
                        Console.WriteLine("Ball is colliding! Direction is " + dir);    
                    }

                    writer.Put(fromPeer.Id);
                    writer.Put(input);
                    writer.Put(ballPosX);
                    writer.Put(ballPosZ);
                    server.SendToAll(writer, DeliveryMethod.ReliableOrdered);
                    writer.Reset();
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
