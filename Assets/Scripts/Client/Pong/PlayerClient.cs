using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClient : MonoBehaviour
{
    private Client client;

    public GameObject ball;
    public GameObject newBall;

    private int speed = 1;
    private int id;

    public bool Collision;

    private Transform ballSpawnPoint1;
    private Transform ballSpawnPoint2;

    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        client = GameObject.FindGameObjectWithTag("Client").GetComponent<Client>();
        client.player = this;
        id = client.id;
        Debug.Log(id);

        gm = FindObjectOfType<GameManager>();
        ballSpawnPoint1 = gm.playerOne.transform.Find("BallSpawnPoint");
        ballSpawnPoint2 = gm.playerTwo.transform.Find("BallSpawnPoint");

        client.SendPlayerStartCoordinates(transform.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Move left/right
        if (Input.GetKey(KeyCode.D))
            client.SendInput("D", Collision);
        else if (Input.GetKey(KeyCode.A))
            client.SendInput("A", Collision);

        if (Input.GetKeyUp(KeyCode.Space))
            client.SendInput("SHOOT", Collision);
    }

    public void CreateBall(int i)
    {
        if (i == 0)
        {
            newBall = Instantiate(ball, ballSpawnPoint1);
            newBall.GetComponent<Ball>().dir = Ball.Direction.Up;
            client.SendBallStartCoordinates(ballSpawnPoint1.position);
        }
        else
        {
            newBall = Instantiate(ball, ballSpawnPoint2);
            newBall.GetComponent<Ball>().dir = Ball.Direction.Down;
            client.SendBallStartCoordinates(ballSpawnPoint2.position);
        }
    }

    //Voitais kokeilla myös OnTriggerEnter jos tämä ei toimi (colliderit triggereiksi)
    private void OnTriggerEnter(Collider collision)
    {
        this.Collision = true;
        print("Colliding");
        //If collision with victory wall, give point
    }
}
