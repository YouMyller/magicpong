using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClient : MonoBehaviour
{
    private Client client;

    public Ball ball;

    private int speed = 1;
    private int id;

    private bool collision;

    private Transform ballSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        client = GameObject.FindGameObjectWithTag("Client").GetComponent<Client>();
        client.player = this;
        id = client.id;
        Debug.Log(id);

        client.SendPlayerStartCoordinates(transform.position);

        ballSpawnPoint = GameObject.FindGameObjectWithTag("BallSpawnPoint").transform;      //Doesn't actually exist
        //Spawn ball
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Move left/right
        if (Input.GetKey(KeyCode.D))
        {
            client.SendInput("D", collision);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            client.SendInput("A", collision);
        }
    }
}
