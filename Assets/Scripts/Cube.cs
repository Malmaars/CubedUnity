using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public GameObject visual;
    Character owner;

    List<Character> currentInhabitants;

    //tracks the neighbors of this cube. 0 is the neighbor to the right of the cube, 1 is above, 2 is to left, 3 is down
    public Cube[] neighbors = new Cube[4];

    // Start is called before the first frame update
    void Awake()
    {
        visual = this.gameObject;
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
