using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using NaughtyAttributes;

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


public class Character : MonoBehaviour, ICharacter, IInventory, IServicable
{
    public GameObject actor { get; set; }
    public Animator animator { get; set; }
    public Cube currentRoom { get; set; }

    public Cube home;

    [field: SerializeField] public Need[] myNeeds { get; set; } = new Need[] {  new Need("hunger", NeedType.hunger),
                                                                                new Need("social", NeedType.social),
                                                                                new Need("comfort", NeedType.comfort),
                                                                                new Need("energy", NeedType.energy)};

    public List<InventoryItem> inventory { get; set; } = new List<InventoryItem>();

    public Character target;
    public StateMachine sm;

    public bool interacting, confused, resetTree;

    public string currentState, currentBehaviour;

    //service variables
    public Character asker { get; set; }
    public Service[] services { get; set; }

    //services that the character can provide for themselves
    public Service[] personalServices;

    public Service GetService(Character _asker, int _index) 
    {
        asker = _asker;
        interacting = true;
        return services[_index]; 
    }
    public void SetAsker(Character _asker)
    {
        asker = _asker;
        beingUsed = true;
    }

    public void EndService() 
    {
        interacting = false;
    }

    public bool beingUsed { get; set; }

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
        Initialize();
    }

    protected virtual void Initialize()
    {
        currentRoom = transform.parent.parent.GetComponent<Cube>();
        currentRoom.currentInhabitants.Add(this);

        home = currentRoom;

        actor = this.gameObject;
        animator = GetComponent<Animator>();

        personalServices = new Service[]{
            new Service(this, NeedType.social, 15,
                        new Sequence(
                            new LookForTarget(this),
                            new LockTarget(this),
                            new GoToTarget(this),
                            new FaceOwnerAndTarget(this),
                            new Talk(this)))
        };

        InventoryItem bed = new InventoryItem(Resources.Load("Items/Bed") as GameObject, "Bed");
        //TODO add a node to go home
        bed.SetServices(new Service[] {new Service(bed, NeedType.energy, 100, new ServiceNode(bed, new Sequence(new GoHome(bed), new CharacterToCenterOfRoom(bed), new SleepOnItem(bed))))});
        AddItemToInventory(bed);

        //The interruptor is so a character can break out of the tree if neccesary
        sm = new StateMachine(new Interruptor(new CheckReset(this), new RemoveReset(this),
                                    new Interruptor(new CheckOccupied(this), new Selector(new LeaveRoom(this), new Irritated(this)),
                                        new Interruptor(new CheckConfused(this), new Sequence(new WalkToNearestRoom(this), new Irritated(this)),
                                            new Sequence(
                                                new ChooseService(this), 
                                                new Idle(this))
                                                )
                                            )
                                        ),
                                this);

    }

    // Update is called once per frame
    void Update()
    {
        LogicUpdate();
    }

    protected virtual void LogicUpdate()
    {
        sm.StateUpdate();

        currentState = sm.currenstate.GetType().ToString();
        //Debug.Log(actor.name + ". Current state: " + sm.currenstate.GetType());

        foreach (Need need in myNeeds)
        {

            //lower needs by time
            need.AddToMeter(-Time.deltaTime * 0.5f);
            need.CalculatePriority();
        }
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

    public void ResolveService(Service s)
    {
        foreach(Need n in myNeeds)
        {
            if (n.type == s.serviceType)
            {
                n.AddToMeter(s.serviceAmount);
                break;
            }
        }
    }

    public void ResolveService(NeedType type, float amount)
    {
        foreach (Need n in myNeeds)
        {
            if (n.type == type)
            {
                n.AddToMeter(amount);
                break;
            }
        }
    }
}
