using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;

public class PlayerServer : MonoBehaviour
{
    //private Server 
    private Transform spawnPoint1;
    private Transform spawnPoint2;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint1 = GameObject.FindGameObjectWithTag("spawnPoint1").transform;
        spawnPoint2 = GameObject.FindGameObjectWithTag("spawnPoint2").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
