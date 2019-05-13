using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using LiteNetLib;
using LiteNetLib.Utils;

public class Client : MonoBehaviour
{
    private NetManager client;
    private EventBasedNetListener listener;

    private Button sendButton;

    private InputField text;
    private InputField message;
    private string messageText;

    NetPeer peer;

    // Start is called before the first frame update
    void Start()
    {
        //sendButton = GameObject.Find("SendMessage").GetComponent<Button>();
        message = GameObject.Find("MessageField").GetComponent<InputField>();
        
        text = GameObject.Find("InputFieldIP").GetComponent<InputField>();
        text.text = NetUtils.GetLocalIp(LocalAddrType.IPv4);
        //ipAddress = NetUtils.GetLocalIp(LocalAddrType.IPv4);

        listener = new EventBasedNetListener();
        client = new NetManager(listener);


        
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

        peer = client.FirstPeer;

        listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod) =>
        {
            string ploo = dataReader.GetString();

            ChatInput(ploo);
            
            //Console.WriteLine("We got: {0}", dataReader.GetString(100 /* max length of string */));
            dataReader.Recycle();
        };
    }

    public void ConnectToServer()
    {
        client.Start();
        client.Connect(text.text /* host ip or name */, 2310 /* port */, "SomeConnectionKey" /* text key or NetDataWriter */);
    }

    /// <summary>
    /// Sets most recent message text into chat input
    /// </summary>
    /// <param name="kakka"></param>
    private void ChatInput(string kakka)
    {
        Text text = GameObject.Find("ChatFieldText").GetComponent<Text>();

        text.text = kakka;
    }

    /// <summary>
    /// Sends message to server
    /// </summary>
    public void SendMessage()
    {
        var writer = new NetDataWriter();
        messageText = message.text;
        Debug.Log(messageText);
        writer.Put(messageText);
        Debug.Log(writer);
        Debug.Log(peer);
        peer.Send(writer, DeliveryMethod.ReliableOrdered);
        writer.Reset();
    }

    private void OnDestroy()
    {
        if (client != null)
            client.Stop();
    }
}
