using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType { Normal, Heal, Special }

public class TurnUI : MonoBehaviour
{
    //Both players should have this individually

    public AttackType attack;

    TurnManager tm;

    [HideInInspector]
    public bool playerReady;

    GameObject ui;

    // Start is called before the first frame update
    void Start()
    {
        tm = FindObjectOfType<TurnManager>();
    }

    public void NormalAttack()
    {
        attack = AttackType.Normal;
    }

    public void Heal()
    {
        attack = AttackType.Heal;
    }

    public void SpecialAttack()
    {
        attack = AttackType.Special;
    }

    void InformManager()
    {
        tm.CommunicateAttack(this, attack);

        playerReady = true;
        ui.SetActive(false);
    }
}
