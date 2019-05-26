using UnityEngine;

public class Ball : MonoBehaviour
{
    private Client client;

    bool collision;

    public enum Direction { Up, Down }
    public Direction dir;


    // Start is called before the first frame update
    void Start()
    {
        dir = Direction.Up;
        client = GameObject.FindGameObjectWithTag("Client").GetComponent<Client>();
        client.SendBallStartCoordinates(transform.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(dir == Direction.Up)
            transform.Translate(Vector3.forward / 4);
        else if(dir == Direction.Down)
            transform.Translate(Vector3.back / 4);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (dir == Direction.Up)
            {
                client.GivePoint("POINT", true);       //if true, give player 1 point
                Destroy(gameObject);
            }
            else if (dir == Direction.Down)
            {
                client.GivePoint("POINT", false);       //if false, give player 2 point
                Destroy(gameObject);
            }
        }
        else
        {
            this.collision = true;
            client.UpdateBallCoordinates(transform.position, "BALL MOVE", collision);
        }
    }
}
