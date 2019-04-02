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

    private void OnCollisionEnter(Collision collision)
    {
        //Assign attack power & type to attack
    }
}
