using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private PlayerClient playerClient;
    private Client client;
    private int speed = 1;
    public byte id;

    bool collision;

    public enum Direction { Up, Down }
    public Direction dir;


    // Start is called before the first frame update
    void Start()
    {
        dir = Direction.Up;
        client = GameObject.FindGameObjectWithTag("Client").GetComponent<Client>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(dir == Direction.Up)
        {
            transform.Translate(Vector3.forward / 4);
        }
        else if(dir == Direction.Down)
        {
            transform.Translate(Vector3.back / 4);
        }

        if (client.receivedNewPosition)
        {
            client.UpdateBallCoordinates(transform.position, "BALL MOVE", false);
            client.receivedNewPosition = false;
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        collision = true;
        client.UpdateBallCoordinates(transform.position, "BALL MOVE", collision);
        print("Ball colliding");
    }

    private void OnTriggerEnter(Collider col)
    {
        collision = true;
        client.UpdateBallCoordinates(transform.position, "BALL MOVE", collision);
        print("Ball colliding at trigger");

        //If collision with victory wall, give point
    }
}
