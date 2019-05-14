using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LiteNetLib;
using LiteNetLib.Utils;

public class Client : MonoBehaviour
{
    private NetManager client;
    private EventBasedNetListener listener;
    private NetPeer peer;
    public Player player;

    private GameObject eventSystem;
    private Canvas canvas; 

    private InputField text;
    private InputField message;
    private string messageText;

    public int id;

    private void Awake()
    {   
        canvas = FindObjectOfType<Canvas>();
        eventSystem = GameObject.Find("EventSystem");
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(canvas);
        DontDestroyOnLoad(eventSystem);
    }

    // Start is called before the first frame update
    void Start()
    {
        message = GameObject.Find("MessageField").GetComponent<InputField>();

        text = GameObject.Find("InputFieldIP").GetComponent<InputField>();
        text.text = NetUtils.GetLocalIp(LocalAddrType.IPv4);

        listener = new EventBasedNetListener();
        client = new NetManager(listener);
    }

    // Update is called once per frame
    void Update()
    {
        client.PollEvents();
        //Thread.Sleep(15);

        peer = client.FirstPeer;

        listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod) =>
        {
            string tempInput = dataReader.GetString();     
            
            if (tempInput == "0" || tempInput == "1")
            {
                id = int.Parse(tempInput);
                Debug.Log(id);
            }
            if (tempInput != "MOVEMENT: 0" || tempInput != "MOVEMENT: 1")
            {
                float tempPlayerInput = dataReader.GetFloat();
            }
            /*
            if (tempInput == "CHAT")
            {
                ChatInput(tempInput);
            }
            */
            
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
        writer.Put(messageText);
        peer.Send(writer, DeliveryMethod.ReliableOrdered);
        writer.Reset();
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(1);
    }

    private void OnDestroy()
    {
        if (client != null)
        {
            peer.Disconnect();
            client.Stop();
        }
    }
}
