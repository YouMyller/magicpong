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
    private GameManager gameManager;
    private NetManager client;
    private EventBasedNetListener listener;
    private NetPeer peer;
    public PlayerClient player;

    private GameObject eventSystem;
    private Canvas canvas;

    private InputField text;
    private InputField message;
    private string messageText;

    public Vector3 spawnPoint;

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
            string tempInput = "";

            //if(dataReader.GetFloatArray() != null)
            //{
            float posX = dataReader.GetFloat();           //This worked
            float posY = dataReader.GetFloat();
            float posZ = dataReader.GetFloat();

            spawnPoint = new Vector3(posX, posY, posZ);
            print(spawnPoint);
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            gameManager.SpawnPlayers(spawnPoint);

            /*print("mnöh");
                float[] positions = dataReader.GetFloatArray();
                spawnPoint = new Vector3(positions[0], positions[1], positions[2]);
                gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
                gameManager.SpawnPlayers(spawnPoint);*/
            //}

            if (dataReader.GetString() != null)
            {
                print("jhagj");
                tempInput = dataReader.GetString();
            }

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

    public void ConnectToServer(bool server)
    {
        if (server)
        {
            client.Start();
            client.Connect("localhost" /* host ip or name */, 2310 /* port */, "SomeConnectionKey" /* text key or NetDataWriter */);
        }
        else
        {
            client.Start();
            client.Connect(text.text /* host ip or name */, 2310 /* port */, "SomeConnectionKey" /* text key or NetDataWriter */);
        }
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

    public void SendStartCoordinates(Vector3 pos)
    {
        var writer = new NetDataWriter();
        float[] positions = new float[3];

        //positions[0] = pos.x;
        //positions[1] = pos.y;
        //positions[2] = pos.z;

        writer.Put(pos.x);
        writer.Put(pos.y);
        writer.Put(pos.z);
        //writer.PutArray(positions);
        peer.Send(writer, DeliveryMethod.ReliableOrdered);
        writer.Reset();
    }

    public void SendInput()
    {

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
