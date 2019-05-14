using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Client client;
    public GameObject atk;

    public GameObject player;
    //public GameObject player2;

    private Vector3 spawnPoint;

    Vector3 attackStartPos;
    Quaternion attackRot;
    
    Quaternion chara1Rot;
    Quaternion chara2Rot;

    int id;
    // Start is called before the first frame update
    void Start()
    {
        client = GameObject.FindGameObjectWithTag("Client").GetComponent<Client>();

        Instantiate(player, spawnPoint, Quaternion.identity);

        if (client.id == 1)
        {
            Instantiate(player, spawnPoint + new Vector3(spawnPoint.x, spawnPoint.y, spawnPoint.z + 50), chara1Rot);
        }
    }
}
