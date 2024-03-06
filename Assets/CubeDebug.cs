using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDebug : MonoBehaviour
{
    private void OnMouseDown()
    {
        //select the cube for the character to walk to, simply by clicking on it

        if (Blackboard.debugMode)
            Blackboard.currentTarget = GetComponent<Cube>();
    }
}
