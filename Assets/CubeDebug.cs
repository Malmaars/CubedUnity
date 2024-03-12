using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDebug : MonoBehaviour
{
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (Blackboard.debugMode)
                Blackboard.currentTarget = GetComponent<Cube>();
        }
    }
}
