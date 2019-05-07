using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject atk;

    public GameObject character1;
    public GameObject character2;

    Vector3 attackStartPos;
    Quaternion attackRot;

    Vector3 chara1Pos;
    Quaternion chara1Rot;
    Vector3 chara2Pos;
    Quaternion chara2Rot;

    // Start is called before the first frame update
    void Start()
    {
        chara1Rot = character1.transform.rotation;
        chara2Rot = character2.transform.rotation;
        attackRot = atk.transform.rotation;

        Instantiate(character1, chara1Pos, chara1Rot);
        Instantiate(character2, chara2Pos, chara2Rot);

    }
}
