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
    }

    // Update is called once per frame
    void Update()
    {
        client.PollEvents();

        peer = client.FirstPeer;

        listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod) =>
        {
            int i;
            string tempInput = "";

            i = dataReader.GetInt();

            if (i == 0 || i == 1)
                id = i;

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
                }
                else if (i == 1 && tempInput == "MOVE")
                {
                    float posX = dataReader.GetFloat(); 
                    gameManager.playerTwo.transform.position = new Vector3(posX, gameManager.playerTwo.transform.position.y, gameManager.playerTwo.transform.position.z);
                    float posZ = dataReader.GetFloat();
                    gameManager.playerTwo.transform.position = new Vector3(gameManager.playerTwo.transform.position.x, gameManager.playerTwo.transform.position.y, posZ);
                }

                if (tempInput == "BALL UP")
                {
                    player.newBall.GetComponent<Ball>().dir = Ball.Direction.Up;

                    float posX = dataReader.GetFloat();
                    player.newBall.transform.position = new Vector3(posX, player.newBall.transform.position.y, player.newBall.transform.position.z);
                    float posZ = dataReader.GetFloat();
                    player.newBall.transform.position = new Vector3(player.newBall.transform.position.x, player.newBall.transform.position.y, posZ);
                }
                else if (tempInput == "BALL DOWN")
                {
                    player.newBall.GetComponent<Ball>().dir = Ball.Direction.Down;

                    float posX = dataReader.GetFloat();
                    player.newBall.transform.position = new Vector3(posX, player.newBall.transform.position.y, player.newBall.transform.position.z);
                    float posZ = dataReader.GetFloat();
                    player.newBall.transform.position = new Vector3(player.newBall.transform.position.x, player.newBall.transform.position.y, posZ);
                }

                if(tempInput == "UPDATE POINTS")
                {
                    if(i == 0)
                    {
                        gameManager.pOnePoints++;
                        gameManager.UpdateUI();
                    }
                    else
                    {
                        gameManager.pTwoPoints++;
                        gameManager.UpdateUI();
                    }
                }

                if(tempInput == "WINNER")
                {
                    gameManager.EndGame(i);
                    player.GameOver = true;
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

    public void SendBallStartCoordinates(Vector3 pos)
    {
        if (id == 0)
        {
            var writer = new NetDataWriter();

            writer.Put("SET BALL POS");
            writer.Put(false);
            writer.Put(pos.x);
            writer.Put(pos.y);
            writer.Put(pos.z);
            peer.Send(writer, DeliveryMethod.ReliableOrdered);
            writer.Reset();
        }
    }

    public void UpdateBallCoordinates(Vector3 pos, string input, bool collision)
    {
        if (id == 0)
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
    }

    public void SendInput(string input, bool collision)
    {
        if (collision && input != prevInput)
        {
            collision = false;
            player.Collision = false;
        }

        var writer = new NetDataWriter();

        writer.Put(input);
        writer.Put(collision);
        peer.Send(writer, DeliveryMethod.ReliableOrdered);
        writer.Reset();

        prevInput = input;
    }

    public void GivePoint(string input, bool playerOne)
    {
        if (id == 0)
        {
            var writer = new NetDataWriter();

            writer.Put(input);
            writer.Put(playerOne);
            peer.Send(writer, DeliveryMethod.ReliableOrdered);
            writer.Reset();
        }
    }

    public void DeclareWinner(string input, bool pOneWins)
    {
        if(id == 0)
        {
            var writer = new NetDataWriter();

            writer.Put(input);
            writer.Put(pOneWins);
            peer.Send(writer, DeliveryMethod.ReliableOrdered);
            writer.Reset();
        }
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
