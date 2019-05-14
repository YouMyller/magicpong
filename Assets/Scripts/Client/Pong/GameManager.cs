using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Client client;

    public GameObject player;
    public GameObject player2;

    //private Vector3 spawnPoint;

    Vector3 attackStartPos;
    Quaternion attackRot;
    
    Quaternion chara1Rot;
    Quaternion chara2Rot;

    int id;

    private void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        client = GameObject.FindGameObjectWithTag("Client").GetComponent<Client>();
    }

    public void SpawnPlayers(Vector3 spawnPoint)
    {
        print("Spanw players");

        if (client.id == 0)
        {
            Instantiate(player, spawnPoint, Quaternion.identity);
        }
        else if (client.id == 1)
        {
            Instantiate(player2, spawnPoint + new Vector3(spawnPoint.x, spawnPoint.y, spawnPoint.z + 50), chara1Rot);
        }
    }
}
