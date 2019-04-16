using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //Stats         
    int hp;
    [HideInInspector]
    public int atk;
    int def;

    [HideInInspector]
    public int curHp;
    [HideInInspector]
    public int curDef;

    int speed = 1;

    public enum Spell { Kakka, Fart };
    public Spell spell;

    public enum Type { Test, Water, Fire };
    public Type type;

    int effectPower;

    Attack move;

    bool usingSpecial;

    // Start is called before the first frame update
    void Start()
    {
        //Init
        //Call stats from db?
        curHp = hp;
        curDef = def;
    }

    void Update()
    {
        if (usingSpecial == true)
            curDef = def / 2;
        else
            curDef = def;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Move left/right
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate((Vector2.right / 10) * speed);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate((Vector2.left / 10) * speed);
        }
    }

    public void SetMove(AttackType type)      
    {
        //Check this Character's stats from db

        //move.AttackEffect(this, db.attackPower, db.healPower, db.specialPower);       //Use this once there is a db we can take values from

        if (type == AttackType.Special)
            usingSpecial = true;
        else
            usingSpecial = false;
    }
}
