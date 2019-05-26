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
    public PlayerClient playerClient;

    private InputField text;
    private InputField message;
    private string messageText;

    public Vector3 position;

    public int id;

    public bool server;
    public bool receivedNewPosition;

    private string prevInput;

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

        receivedNewPosition = true;
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

            if (playerClient != null)
            {
                gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>(); //This should only happen once
                PlayerClient playerC1 = gameManager.playerOne.GetComponent<PlayerClient>();
                PlayerClient playerC2 = gameManager.playerOne.GetComponent<PlayerClient>();
                tempInput = dataReader.GetString();
                print("Received input from server: " + tempInput);

                if (i == 0 && tempInput == "MOVE")
                {
                    float posX = dataReader.GetFloat();
                    gameManager.playerOne.transform.position = new Vector3(posX, gameManager.playerOne.transform.position.y, gameManager.playerOne.transform.position.z);
                    float posZ = dataReader.GetFloat();
                    gameManager.playerOne.transform.position = new Vector3(gameManager.playerOne.transform.position.x, gameManager.playerOne.transform.position.y, posZ);
                }
                else if (i == 1 && tempInput == "MOVE")
                {
                    float posX = dataReader.GetFloat();
                    gameManager.playerTwo.transform.position = new Vector3(posX, gameManager.playerTwo.transform.position.y, gameManager.playerTwo.transform.position.z);
                    float posZ = dataReader.GetFloat();
                    gameManager.playerTwo.transform.position = new Vector3(gameManager.playerTwo.transform.position.x, gameManager.playerTwo.transform.position.y, posZ);
                }

                //Receives balls movement based on the player id
                if (i == 0 && tempInput == "BALL MOVE" && gameManager.playerOne.GetComponent<PlayerClient>().ballBackToStart == false)
                {
                    print("Wadap ball is moving");

                    //Move player 1's own ball
                    float posX = dataReader.GetFloat();
                    playerC1.balls[0].transform.position = new Vector3(posX, playerC1.newBall1.transform.position.y, playerC1.newBall1.transform.position.z);
                    float posZ = dataReader.GetFloat();
                    playerC1.balls[0].transform.position = new Vector3(playerC1.newBall1.transform.position.x, playerC1.newBall1.transform.position.y, posZ);
                    receivedNewPosition = true;
                }
                else if (playerC1.ballBackToStart)
                {
                    receivedNewPosition = true;
                    playerC1.ballBackToStart = false;
                }

                if (i == 1 && tempInput == "BALL MOVE" && gameManager.playerTwo.GetComponent<PlayerClient>().ballBackToStart == false)
                {
                    print("Wadap ball is moving");

                    //Move player 2's own ball
                    float posX = dataReader.GetFloat();
                    playerC2.balls[0].transform.position = new Vector3(posX, playerC2.newBall1.transform.position.y, playerC2.newBall1.transform.position.z);
                    float posZ = dataReader.GetFloat();
                    playerC2.balls[0].transform.position = new Vector3(playerC2.newBall1.transform.position.x, playerC2.newBall1.transform.position.y, posZ);
                    receivedNewPosition = true;
                }
                else if (playerC2.ballBackToStart)
                {
                    receivedNewPosition = true;
                    playerC2.ballBackToStart = false;
                }
            }

            dataReader.Recycle();
        };
    }

    /// <summary>
    /// Connects to server
    /// </summary>
    public void ConnectToServer()
    {
        client.Start();
        client.Connect(text.text /* host ip or name */, 2310 /* port */, "SomeConnectionKey" /* text key or NetDataWriter */);      //2310 is school port
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
        writer.Put(false);
        writer.Put(pos.x);
        writer.Put(pos.y);
        writer.Put(pos.z);
        peer.Send(writer, DeliveryMethod.ReliableOrdered);
        writer.Reset();
    }

    public void UpdateBallCoordinates(Vector3 pos, string input, bool collision)
    {
        var writer = new NetDataWriter();

        writer.Put(input);
        writer.Put(collision);
        writer.Put(pos.x);
        writer.Put(pos.y);
        writer.Put(pos.z);

        peer.Send(writer, DeliveryMethod.ReliableOrdered);
        writer.Reset();
    }

    public void SendInput(string input, bool collision)
    {
        if (collision && input != prevInput)
        {
            collision = false;
            playerClient.Collision = false;
        }

        var writer = new NetDataWriter();

        writer.Put(input);
        writer.Put(collision);
        peer.Send(writer, DeliveryMethod.ReliableOrdered);
        writer.Reset();

        prevInput = input;
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
