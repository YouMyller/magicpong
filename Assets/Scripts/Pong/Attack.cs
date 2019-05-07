using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    TurnManager tm;

    public enum Direction { TowardsP1, TowardsP2 };
    public Direction dir;

    Vector2 rotationY;       

    float speed = 1;
    float changedRot;

    bool colliding;

    int p1_atkPower;
    int p1_healPower;
    int p1_specialPower;

    int p2_atkPower;
    int p2_healPower;
    int p2_specialPower;

    Character playerOne;
    Character playerTwo;
    Character attacker;

    // Start is called before the first frame update
    void Start()
    {
        dir = Direction.TowardsP1;
        rotationY = Vector2.one.normalized;
        changedRot = 90;

        playerOne = GameObject.Find("Player1").GetComponent<Character>();
        playerTwo = GameObject.Find("Player2").GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        //Move 
        //transform.Translate(rotationY * speed * Time.deltaTime);
        if(dir == Direction.TowardsP1)
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

                if (dir == Direction.TowardsP1)
                {
                    dir = Direction.TowardsP2;
                }
                else
                {
                    dir = Direction.TowardsP1;
                }

                speed *= 1.2f;

                changedRot = Random.Range(0, 90);
                transform.Rotate(transform.rotation.x, changedRot, transform.rotation.z, Space.Self);

                attacker = collision.gameObject.GetComponent<Character>();              
            }

            if (collision.gameObject.tag == "Wall1")
            {
                colliding = true;

                if (dir == Direction.TowardsP1)
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

                if (dir == Direction.TowardsP1)
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

    public void AttackEffect(Character player, int atk, int heal, int special)
    {
        if (player == playerOne)
        {
            p1_atkPower = atk;
            p1_healPower = heal;
            p1_specialPower = special;
        }
        else if (player == playerTwo)
        {
            p2_atkPower = atk;
            p2_healPower = heal;
            p2_specialPower = special;
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
            //Hurt the other character & stuffs

            if(attacker == playerTwo)
            {
                if(p2_atkPower > 0)
                    playerOne.curHp -= p2_atkPower + playerTwo.atk - playerOne.curDef;

                if(p2_specialPower > 0)
                    playerOne.curHp -= p2_specialPower + playerTwo.atk - playerOne.curDef;

                if(p2_healPower > 0)
                    playerTwo.curHp += p2_healPower;

            }
            else if(attacker == playerOne)
            {
                if(p1_atkPower > 0)
                    playerTwo.curHp -= p1_atkPower + playerOne.atk - playerTwo.curDef;

                if (p1_specialPower > 0)
                    playerTwo.curHp -= p1_specialPower + playerOne.atk - playerTwo.curDef;

                if (p1_healPower > 0)
                    playerOne.curHp += p1_healPower;
            }

            tm.NextTurn();
        }
    }
}
