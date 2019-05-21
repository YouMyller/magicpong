using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Client client;
    private int speed = 1;

    bool collision;


    // Start is called before the first frame update
    void Start()
    {
        client = GameObject.FindGameObjectWithTag("Client").GetComponent<Client>();
        client.SendBallStartCoordinates(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        client.SendInput("BALL MOVE", collision);
    }

    //Voitais kokeilla myös OnTriggerEnter jos tämä ei toimi (colliderit triggereiksi)
    private void OnCollisionEnter(Collision collision)
    {
        this.collision = true;

        //If collision with victory wall, give point
    }

    private void OnCollisionExit(Collision collision)
    {
        this.collision = false;
    }

    //Kokeillaan tätä jos muu ei toimi
    /*bool IsColliding(Vector3 startPoint, Vector3 endPoint, float width)
    {
        return Physics.CheckCapsule(startPoint, endPoint, width);
    }*/
}
