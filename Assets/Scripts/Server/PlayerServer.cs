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

    // Start is called before the first frame update
    void Awake()
    {
        client = GameObject.FindGameObjectWithTag("Client").GetComponent<Client>();

        spawnPoint1 = GameObject.FindGameObjectWithTag("SpawnPoint1").transform;
        spawnPoint2 = GameObject.FindGameObjectWithTag("SpawnPoint2").transform;

        client.SendStartCoordinates(spawnPoint1.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
