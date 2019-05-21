using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;

public class PlayerServer : MonoBehaviour
{
    private Client client;

    private Transform spawnPoint1;
    private Transform spawnPoint2;

    bool server;

    // Start is called before the first frame update
    void Awake()
    {
        client = GameObject.FindGameObjectWithTag("Client").GetComponent<Client>();

        spawnPoint1 = GameObject.FindGameObjectWithTag("SpawnPoint1").transform;
        spawnPoint2 = GameObject.FindGameObjectWithTag("SpawnPoint2").transform;
        server = client.server;
        SendStartPositions(server);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendStartPositions(bool s)
    {
        if (s)
        {
            client.SendStartCoordinates(spawnPoint1.position, "0");
            client.SendStartCoordinates(spawnPoint2.position, "1");
        }
    }
}
