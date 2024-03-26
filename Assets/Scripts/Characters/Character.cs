using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public enum characterType
{
    artist,
    astronaut,
    pirate,
    robot,
    builder,
    samurai,
    skeletonking
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

    public SerializableNode[] tree;
}


public class Character : MonoBehaviour, ICharacter, IInventory
{
    public GameObject actor { get; set; }
    public Animator animator { get; set; }
    public Cube currentRoom { get; set; }
    public NeedType serviceType { get; set; }
    public int serviceAmount { get; set; }

    public List<InventoryItem> inventory { get; set; }


    public Character target;
    public StateMachine sm;

    public bool interacting;
    public bool confused;
    public bool resetTree;

    public string currentState;

    public Cube RequestRoom()
    {
        if (Blackboard.debugMode)
            return Blackboard.currentTarget;

        for(int i = 0; i < currentRoom.neighbors.Length; i++)
        {
            if (currentRoom.neighbors[i] != null)
                break;

            if (i == currentRoom.neighbors.Length - 1)
            {
                Debug.Log("The room has no neighbours, so I can't request a room");
                return null;
            }
        }

        int index = UnityEngine.Random.Range(1, Blackboard.allCubes.Count);
        int baseIndex = index;
        Cube room = Blackboard.allCubes[index];
        while(!FindRoom.CheckIfPossible(currentRoom, room))
        {
            index++;
            if (index >= Blackboard.allCubes.Count)
                index = 0;

            if (index == baseIndex)
                return null;

            if(Blackboard.allCubes[index] == currentRoom)
            {
                index++;
                if (index >= Blackboard.allCubes.Count)
                    index = 0;
            }

            room = Blackboard.allCubes[index];
        }

        return room;
        //return Blackboard.allCubes[12];
    }

    //a character will have some stats and a behaviourtree.
    //maybe an inventory as well

    characterType myType;

    //characters need to be able to react to other characters in proximity. How will they check that?
    //Perhaps characters can know in which room they are, and the room also track which inhabitants are in the room

    // Start is called before the first frame update
    void Start()
    {
        currentRoom = transform.parent.parent.GetComponent<Cube>();
        currentRoom.currentInhabitants.Add(this);

        actor = this.gameObject;
        animator = GetComponent<Animator>();

        //The interruptor is so a character can break out of the tree if neccesary
        sm = new StateMachine(new Interruptor(new CheckReset(this), new RemoveReset(this),
                                    new Interruptor(new CheckConfused(this), new Sequence(new WalkToNearestRoom(this), new Irritated(this)),
                                        new Selector(
                                            new ForcedSequence(
                                                new Idle(),
                                                //sequence for talking to another character
                                                new Sequence(
                                                    new LookForTarget(this),
                                                    new LockTarget(this),
                                                    new GoToTarget(this),
                                                    new FaceOwnerAndTarget(this),
                                                    new Talk(this)),
                                                new GoToRandomRoom(this)
                                                    )))),
                                this);
    }

    // Update is called once per frame
    void Update()
    {
        sm.StateUpdate();

        currentState = sm.currenstate.GetType().ToString();
        //Debug.Log(actor.name + ". Current state: " + sm.currenstate.GetType());
    }

    public void ResetTree()
    {
        resetTree = true;
    }

    public void SetRoom(Cube newRoom)
    {
        currentRoom.currentInhabitants.Remove(this);
        currentRoom = newRoom;
        currentRoom.currentInhabitants.Add(this);
        actor.transform.parent = newRoom.visual.transform.GetChild(2);
    }

    public void RemoveTarget()
    {
        target.sm.ResetState();
        interacting = false;
        target = null;
    }

    public void AddItemToInventory(InventoryItem _toAdd)
    {
        inventory.Add(_toAdd);
        _toAdd.DisableItem();
    }
}
