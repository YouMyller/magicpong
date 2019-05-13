using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;

public class Client : MonoBehaviour
{
    private NetManager client;
    private EventBasedNetListener listener;
    private NetDataWriter writer;

    // Start is called before the first frame update
    void Start()
    {
        listener = new EventBasedNetListener();
        client = new NetManager(listener);
        writer = new NetDataWriter();

        client.Start();
        client.Connect("127.0.0.1" /* host ip or name */, 2310 /* port */, "SomeConnectionKey" /* text key or NetDataWriter */);

        /*
        while (!Console.KeyAvailable)
        {
            client.PollEvents();
            Thread.Sleep(15);
        }
        */
        
    }

    // Update is called once per frame
    void Update()
    {
        client.PollEvents();
        var peer = client.FirstPeer;

        listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod) =>
        {
            string ploo = dataReader.GetString();
            Debug.Log(ploo);

            //Console.WriteLine("We got: {0}", dataReader.GetString(100 /* max length of string */));
            dataReader.Recycle();
        };

        if (Input.GetKeyDown(KeyCode.Space))
        {
            writer.Put("Hello server");
            peer.Send(writer, DeliveryMethod.ReliableOrdered);
            //client.Stop();
        }
    }

    private void OnDestroy()
    {
        if (client != null)
            client.Stop();
    }
}
