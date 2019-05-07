using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    GameObject myself;

    Vector3 myPos;
    Vector3 enemyPos;

    // Start is called before the first frame update
    void Start()
    {
        //Position is behind "myself"
    }

    // Update is called once per frame
    void Update()
    {
        //Keep camera rotated towards enemy
        Vector3.RotateTowards(myPos, enemyPos, 135, 10);
    }
}
