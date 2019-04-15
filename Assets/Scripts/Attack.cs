using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public enum Direction { Left, Right };
    public Direction dir;

    Vector2 rotationY;       

    float speed = 1;
    float changedRot;

    bool colliding;

    // Start is called before the first frame update
    void Start()
    {
        dir = Direction.Left;
        rotationY = Vector2.one.normalized;
        changedRot = 90;
    }

    // Update is called once per frame
    void Update()
    {
        //Move 
        //transform.Translate(rotationY * speed * Time.deltaTime);
        if(dir == Direction.Left)
        {
            transform.Translate((Vector2.right / 10) * speed);
            transform.rotation = Quaternion.Euler(transform.rotation.x, changedRot, transform.rotation.z);
        }
        else
        {
            transform.Translate((Vector2.left / 10) * speed);
            transform.rotation = Quaternion.Euler(transform.rotation.x, changedRot, transform.rotation.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (colliding == false)
        {
            if (collision.gameObject.tag == "Player")
            {
                colliding = true;

                if (dir == Direction.Left)
                {
                    dir = Direction.Right;
                }
                else
                {
                    dir = Direction.Left;
                }

                speed *= 1.2f;

                changedRot = Random.Range(0, 90);
                transform.Rotate(transform.rotation.x, changedRot, transform.rotation.z, Space.Self);

            }

            if (collision.gameObject.tag == "Wall1")
            {
                colliding = true;

                if (dir == Direction.Left)
                {
                    changedRot = Random.Range(100, 135);
                }
                else
                {
                    changedRot = Random.Range(0, 45);
                }

                transform.Rotate(transform.rotation.x, changedRot, transform.rotation.z, Space.Self);
            }
            else if (collision.gameObject.tag == "Wall2")
            {
                colliding = true;

                if (dir == Direction.Left)
                {
                    changedRot = Random.Range(0, 45);
                }
                else
                {
                    changedRot = Random.Range(100, 135);
                }

                transform.Rotate(transform.rotation.x, changedRot, transform.rotation.z, Space.Self);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Wall1" || collision.gameObject.tag == "Wall2")
        {
            colliding = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Goal")
        {
            //print("Kakka");
        }
        //When enter goal, gooooaaaallll!!!
        //Hurt the hurt character & stuffs
    }
}
