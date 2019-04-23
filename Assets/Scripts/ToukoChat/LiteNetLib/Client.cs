using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using LiteNetLib;

public class Client : MonoBehaviour
{
    NetManager client;
    // Start is called before the first frame update
    void Start()
    {
        EventBasedNetListener listener = new EventBasedNetListener();
        client = new NetManager(listener);

        client.Start();
        client.Connect("localhost" /* host ip or name */, 9050 /* port */, "SomeConnectionKey" /* text key or NetDataWriter */);
        listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod) =>
        {
            //Console.WriteLine("We got: {0}", dataReader.GetString(100 /* max length of string */));
            dataReader.Recycle();
        };

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
        if (Input.GetKey(KeyCode.Space))
        {
            client.Stop();
        }
    }
}
