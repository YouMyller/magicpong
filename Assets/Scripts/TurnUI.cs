using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType { Normal, Defend, Special, Ultra }

public class TurnUI : MonoBehaviour
{
    //Both players should have this individually

    public AttackType attack;

    TurnManager tm;

    [HideInInspector]
    public bool playerReady;

    // Start is called before the first frame update
    void Start()
    {
        tm = FindObjectOfType<TurnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NormalAttack()
    {
        attack = AttackType.Normal;

        tm.CommunicateAttack(this, attack);
    }

    public void Defend()
    {
        attack = AttackType.Defend;

        tm.CommunicateAttack(this, attack);
    }

    public void SpecialAttack()
    {
        attack = AttackType.Special;

        tm.CommunicateAttack(this, attack);
    }

    public void UltraAttack()
    {
        attack = AttackType.Ultra;

        tm.CommunicateAttack(this, attack);
    }
}
