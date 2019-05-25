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

    private Transform ballSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        client = GameObject.FindGameObjectWithTag("Client").GetComponent<Client>();
        client.player = this;
        id = client.id;
        Debug.Log(id);

        client.SendPlayerStartCoordinates(transform.position);

        ballSpawnPoint = GameObject.FindGameObjectWithTag("BallSpawnPoint").transform;
        newBall = Instantiate(ball, ballSpawnPoint);
        client.SendBallStartCoordinates(ballSpawnPoint.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Move left/right
        if (Input.GetKey(KeyCode.D))
        {
            client.SendInput("D", Collision);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            client.SendInput("A", Collision);
        }
    }

    //Voitais kokeilla myös OnTriggerEnter jos tämä ei toimi (colliderit triggereiksi)
    private void OnTriggerEnter(Collider collision)
    {
        this.Collision = true;
        print("Colliding");
        //If collision with victory wall, give point
    }

    //Kokeillaan tätä jos muu ei toimi
    /*bool IsColliding(Vector3 startPoint, Vector3 endPoint, float width)
    {
        return Physics.CheckCapsule(startPoint, endPoint, width);
    }*/
}
