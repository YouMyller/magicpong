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

    private InputField text;
    private InputField message;
    private string messageText;

    public Vector3 position;

    public int id;

    public bool server;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
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
            int i;
            string tempInput = "";

            i = dataReader.GetInt();
            print("Get player id: " + i);

            if (i == 0 || i == 1)
            {
                id = i;
                Debug.Log("Player id: " + id);
            }

            if (player != null)
            {
                gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>(); //This should only happen once
                tempInput = dataReader.GetString();
                if (i == 0 && tempInput == "MOVE")
                {
                    float posX = dataReader.GetFloat();            
                    gameManager.playerOne.transform.position = new Vector3(posX, gameManager.playerOne.transform.position.y, gameManager.playerOne.transform.position.z);
                    float posZ = dataReader.GetFloat();
                    gameManager.playerOne.transform.position = new Vector3(gameManager.playerOne.transform.position.x, gameManager.playerOne.transform.position.y, posZ);
                    Debug.Log("Move player one: " + posX + posZ);
                    print(position);
                }
                else if (i == 1 && tempInput == "MOVE")
                {
                    float posX = dataReader.GetFloat(); 
                    gameManager.playerTwo.transform.position = new Vector3(posX, gameManager.playerTwo.transform.position.y, gameManager.playerTwo.transform.position.z);
                    float posZ = dataReader.GetFloat();
                    gameManager.playerTwo.transform.position = new Vector3(gameManager.playerTwo.transform.position.x, gameManager.playerTwo.transform.position.y, posZ);
                    Debug.Log("Move player two: " + posX + posZ);
                    print(position);
                }

                if(tempInput == "BALL MOVE")
                {
                    float posX = dataReader.GetFloat();
                    player.ball.transform.position = new Vector3(posX, gameManager.playerTwo.transform.position.y, gameManager.playerTwo.transform.position.z);
                    float posZ = dataReader.GetFloat();
                    player.ball.transform.position = new Vector3(gameManager.playerTwo.transform.position.x, gameManager.playerTwo.transform.position.y, posZ);
                }
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

    /// <summary>
    /// Connects to server
    /// </summary>
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

    public void SendPlayerStartCoordinates(Vector3 pos)
    {
        var writer = new NetDataWriter();

        writer.Put("SET POS");
        writer.Put(pos.x);
        writer.Put(pos.y);
        writer.Put(pos.z);
        peer.Send(writer, DeliveryMethod.ReliableOrdered);
        writer.Reset();
    }

    public void SendBallStartCoordinates(Vector3 pos)
    {
        var writer = new NetDataWriter();

        writer.Put("SET BALL POS");
        writer.Put(pos.x);
        writer.Put(pos.y);
        writer.Put(pos.z);
        peer.Send(writer, DeliveryMethod.ReliableOrdered);
        writer.Reset();
    }

    public void SendInput(string input, bool collision)
    {
        var writer = new NetDataWriter();

        writer.Put(input);
        writer.Put(collision);
        peer.Send(writer, DeliveryMethod.ReliableOrdered);
        writer.Reset();
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
