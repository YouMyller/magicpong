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

    public enum Spell { Kakka, Fart };
    public Spell spell;

    public enum Type { Water, Fire };
    public Type type;

    // Start is called before the first frame update
    void Start()
    {
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
        //Move left/right
        if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector2.right);
        }
        else if(Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector2.left);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Assign attack power & type to ball
    }
}
