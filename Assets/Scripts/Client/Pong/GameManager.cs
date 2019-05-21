using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Client client;

    public GameObject player;
    public GameObject playerOne;
    public GameObject playerTwo;

    private Transform spawnPoint1;
    private Transform spawnPoint2;
    
    Quaternion chara1Rot;
    Quaternion chara2Rot;

    // Start is called before the first frame update
    void Start()
    {
        client = GameObject.FindGameObjectWithTag("Client").GetComponent<Client>();
        SpawnPlayers();
    }

    public void SpawnPlayers()
    {
        spawnPoint1 = GameObject.FindGameObjectWithTag("SpawnPoint1").transform;
        spawnPoint2 = GameObject.FindGameObjectWithTag("SpawnPoint2").transform;
        print("Spawn players");

        playerOne = Instantiate(player, spawnPoint1.position, Quaternion.identity);
        playerTwo = Instantiate(player, spawnPoint2.position, chara1Rot);

        if (client.id == 0)
        {
            playerTwo.transform.Find("Main Camera").gameObject.SetActive(false);
            playerTwo.GetComponent<PlayerClient>().enabled = false;
        }
        else if (client.id == 1)
        {
            playerOne.transform.Find("Main Camera").gameObject.SetActive(false);
            playerOne.GetComponent<PlayerClient>().enabled = false;
        }
    }
}
