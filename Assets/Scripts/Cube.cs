using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    Character owner;

    List<Character> currentInhabitants;

    //tracks the neighbors of this cube. 0 is the neighbor above the cube, 1 is to the left, 2 is to the bottom, 3 is to the right
    Cube[] neighbors = new Cube[4];

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //call this to assign a neighbor to the cube
    public void ConnectNeighbor(Cube _cube, int direction)
    {
        if (direction < 0 || direction > neighbors.Length)
        {
            Debug.LogError("This direction does not exist. Not assigning neighbor.");
            return;
        }

        neighbors[direction] = _cube;
    }
}
