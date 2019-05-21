using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClient : MonoBehaviour
{
    private Client client;

    private int speed = 1;
    private int id;

    // Start is called before the first frame update
    void Start()
    {
        client = GameObject.FindGameObjectWithTag("Client").GetComponent<Client>();
        client.player = this;
        id = client.id;
        Debug.Log(id);

        client.SendStartCoordinates(transform.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Move left/right
        if (Input.GetKey(KeyCode.D))
        {
            client.SendInput("D");
        }
        else if (Input.GetKey(KeyCode.A))
        {
            client.SendInput("A");
        }
    }
}
