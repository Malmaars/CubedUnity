using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public enum characterType
{
    artist,
    astronaut
}

public enum characterNeeds
{
    hunger,
    social,
    relax
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
    public NeedType serviceType { get; set; }
    public int serviceAmount { get; set; }

    public Character target;
    public StateMachine sm;

    public bool interacting;

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
        currentRoom.currentInhabitants.Add(this);

        actor = this.gameObject;
        animator = GetComponent<Animator>();

        sm = new StateMachine(new Selector(
                                    new Sequence(
                                        new Idle(),
                                        new Sequence(
                                            new LookForTarget(this),
                                            new LockTarget(this),
                                            new GoToTarget(this),
                                            new Talk(this)),
                                        new GoToRandomRoom(this)
                                            )), 
                                            this);
    }

    // Update is called once per frame
    void Update()
    {
        sm.StateUpdate();

        Debug.Log(actor.name + ". Current state: " + sm.currenstate.GetType());
    }

    public void SetRoom(Cube newRoom)
    {
        currentRoom.currentInhabitants.Remove(this);
        currentRoom = newRoom;
        currentRoom.currentInhabitants.Add(this);
        actor.transform.parent = newRoom.visual.transform.GetChild(2);
    }
}
