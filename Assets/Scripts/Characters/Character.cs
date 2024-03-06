using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public enum characterType
{
    artist,
    astronaut
}

[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/Character", order = 1)]
public class ScriptableCharacter : ScriptableObject
{
    public string characterName;
    public characterType type;
    public GameObject body;
    public Sprite characterSprite;

    //perhaps sliders for personality
}

public class Character : MonoBehaviour, ICharacter
{
    public GameObject actor { get; set; }
    public Animator animator { get; set; }
    public Cube currentRoom { get; set; }

    public Cube RequestRoom()
    {
        if (Blackboard.debugMode)
            return Blackboard.currentTarget;

        return Blackboard.allCubes[Random.Range(1, Blackboard.allCubes.Length)];
        //return Blackboard.allCubes[12];
    }

    //a character will have some stats and a behaviourtree.
    //maybe an inventory as well

    characterType myType;

    //a character should be a child of a cube. This way, if the cube moves, the characters move as well
    Cube currentlyInhabitedCube;

    Node behaviourtree;

    //characters need to be able to react to other characters in proximity. How will they check that?
    //Perhaps characters can know in which room they are, and the room also track which inhabitants are in the room

    // Start is called before the first frame update
    void Start()
    {
        currentRoom = transform.parent.parent.GetComponent<Cube>();
        actor = this.gameObject;
        animator = GetComponent<Animator>();

        behaviourtree = new GoToRoom(this);
    }

    // Update is called once per frame
    void Update()
    {
        behaviourtree.Run();
    }
}
