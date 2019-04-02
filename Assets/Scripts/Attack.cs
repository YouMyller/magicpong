using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public enum Direction { Left, Right };
    public Direction dir;

    Vector2 rotationY;       

    float speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        dir = Direction.Left;
        rotationY = Vector2.one.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        //Move 
        //transform.Translate(rotationY * speed * Time.deltaTime);
        if(dir == Direction.Left)
        {
            transform.Translate((Vector2.right / 10) * speed);
        }
        else
        {
            transform.Translate((Vector2.left / 10) * speed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        float changedRot;

        if (collision.gameObject.tag == "Player")
        {
            //Change direction & rotation when hit by player

            if (dir == Direction.Left)
            {
                dir = Direction.Right;
            }
            else
            {
                dir = Direction.Left;
            }

            changedRot = Random.Range(0, 90);

            transform.Rotate(transform.rotation.x, changedRot, transform.rotation.z, Space.Self);
        }

        if (collision.gameObject.tag == "Wall1")
        {
            changedRot = rotationY.y - 90;      //THESE ARE BROKEN AS HELL & NOT READY OR FUNCTIONAL AT ALL; I REPEAT, AT ALL

            transform.Rotate(transform.rotation.x, changedRot, transform.rotation.z, Space.Self);
        }
        else if(collision.gameObject.tag == "Wall2")
        {
            changedRot = rotationY.y + 180;     //THESE ARE BROKEN AS HELL & NOT READY OR FUNCTIONAL AT ALL; I REPEAT, AT ALL

            transform.Rotate(transform.rotation.x, changedRot, transform.rotation.z, Space.Self);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Goal")
        {
            print("Kakka");
        }
        //When enter goal, gooooaaaallll!!!
        //Hurt the hurt character & stuffs
    }
}
