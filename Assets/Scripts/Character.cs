using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //Stats         //Idea: Statsit vois hakee scriptableista
    int hp;
    int atk;
    int def;
    float speed;

    float moveMin;      //Nää vois hakee automattisesti
    float moveMax;       

    public enum Spell { Kakka, Fart };
    public Spell spell;

    public enum Type { Test, Water, Fire };
    public Type type;

    int effectPower;
    //int damage;
    //int defend;

    // Start is called before the first frame update
    void Start()
    {
        if(type == Type.Test)
        {
            speed = 1;
        }
        //Init
        //Call stats from db?
    }

    void Update()
    {
        //Use spells
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (transform.position.x >= moveMax || transform.position.x <= moveMin)
        //{
            //Move left/right
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate((Vector2.right / 10) * speed);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.Translate((Vector2.left / 10) * speed);
            }
        //}
    }

    public void SetMove(AttackType type)      
    {
        if(type == AttackType.Normal)
        {
            //Check this Character's attack from db
            //effectPower = attackPower from db
        }
        else if (type == AttackType.Defend)
        {
            //Check this Character's defend move from db
            //effectPower = defendPower from db

        }
        else if (type == AttackType.Special)
        {
            //Check this Character's special move from db
            //effectPower = speicalPower from db
        }
        else if (type == AttackType.Ultra)
        {
            //Check this Character's ultra move from db
            //effectPower = ultraPower from db
            //Do the waiting effect somehow
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Assign attack power & type to attack
    }
}
