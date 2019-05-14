using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
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
    }

    void Update()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Move left/right
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate((Vector2.right / 10) * speed);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate((Vector2.left / 10) * speed);
        }
    }
}
