using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClient : MonoBehaviour
{
    private Client client;

    public GameObject ball;
    public GameObject newBall1;
    public GameObject newBall2;

    public List<GameObject> balls = new List<GameObject>();

    public Vector3 ballStartPos;

    private int speed = 1;
    private int id;

    public bool Collision;
    private bool shootingDone;
    public bool ballBackToStart;

    private Transform ballSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        client = GameObject.FindGameObjectWithTag("Client").GetComponent<Client>();
        client.playerClient = this;
        id = client.id;
        //Debug.Log(id);

        client.SendPlayerStartCoordinates(transform.position);
        ballStartPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1.5f);
        shootingDone = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ballStartPos.x = transform.position.x;
        //Move left/right
        if (Input.GetKey(KeyCode.D))
        {
            client.SendInput("D", Collision);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            client.SendInput("A", Collision);
        }

        if (Input.GetKey(KeyCode.Mouse0) && shootingDone == true)
        {
            StartCoroutine(Shoot());
            shootingDone = false;
        }
    }

    private IEnumerator Shoot()
    {
        if (balls.Count < 1)
        {
            newBall1 = Instantiate(ball, ballStartPos, Quaternion.identity);
            balls.Add(newBall1);
        }
        else if (balls.Count >= 1)
        {
            balls[0].transform.position = ballStartPos;
            ballBackToStart = true;
        }

        yield return new WaitForSeconds(0.5f);

        shootingDone = true;
        yield return null;
    }

    //Voitais kokeilla myös OnTriggerEnter jos tämä ei toimi (colliderit triggereiksi)
    private void OnTriggerEnter(Collider collision)
    {
        this.Collision = true;
        print("Colliding");
        //If collision with victory wall, give point
    }
}
