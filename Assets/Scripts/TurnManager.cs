using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    GameObject playerOne, playerTwo;

    TurnUI playerOneTurn, playerTwoTurn;

    Character charaOne, charaTwo;

    Attack atk;

    // Start is called before the first frame update
    void Start()
    {
        playerOne = GameObject.Find("Player1");
        playerTwo = GameObject.Find("Player2");

        playerOneTurn = playerOne.GetComponent<TurnUI>();
        playerTwoTurn = playerTwo.GetComponent<TurnUI>();

        charaOne = playerOne.GetComponent<Character>();
        charaTwo = playerTwo.GetComponent<Character>();
        atk = FindObjectOfType<Attack>();

        charaOne.enabled = false;
        charaTwo.enabled = false;
        atk.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerOneTurn.playerReady == true && playerTwoTurn.playerReady == true)
        {
            //Activate battle!

            //Start networking thingsies

            charaOne.enabled = true;
            charaTwo.enabled = true;
            atk.enabled = true;


        }
    }

    public void CommunicateAttack(TurnUI player, AttackType attack)
    {
        if(player == playerOneTurn)
        {
            charaOne.SetMove(attack);
        }
        else if(player == playerTwoTurn)
        {
            charaTwo.SetMove(attack);
        }
    }
}
