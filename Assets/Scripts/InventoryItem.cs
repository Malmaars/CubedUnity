using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "ScriptableObjects/InventoryItem")]

public class ScriptableItem : ScriptableObject
{
    public string itemName;
    public GameObject visual;
}

public enum itemSize
{
    Small,
    Medium,
    Large
}

[System.Serializable]
public class InventoryItem : IServicable
{
    //Iservicable data
    public Character asker { get; set; }
    public Service[] services { get; set; }

    public bool beingUsed { get; set; }
    public bool placeable, hangable;

    public string name;
    public GameObject visual;
    public Animator animator;

    public itemSize size;

    //I don't yet know if it's better to make presets for known items, perhaps if different scripts want to spawn the same item
    public InventoryItem(GameObject _visual)
    {
        CreateItem(_visual);
        //get the first letter of the name of the object, it will determine its size
        char[] visualName = visual.name.ToCharArray();

        switch (visualName[0])
        {
            case 'S':
                size = itemSize.Small;
                break;
            case 'M':
                size = itemSize.Medium;
                break;
            case 'L':
                size = itemSize.Large;
                break;
        }

        //the name of the item will start from the 3rd letter

        char[] itemName = new char[visualName.Length - 2];
        for(int i = 0; i < itemName.Length; i++)
        {
            itemName[i] = visualName[i + 2];
        }
        name = new string(itemName);
    }

    public void CreateItem(GameObject _visual) 
    {
        visual = Object.Instantiate(_visual);
        animator = visual.GetComponentInChildren<Animator>();
    }

    public void SetServices(Service[] _services)
    {
        services = _services;
    }

    public void DisableItem()
    {
        visual.SetActive(false);
    }

    public void EnableItem()
    {
        visual.SetActive(true);
    }


    public Service GetService(Character _asker, int _index)
    {
        asker = _asker;
        return services[_index];
    }

    public void SetAsker(Character _asker)
    {
        asker = _asker;
        beingUsed = true;
    }

    public void EndService()
    {

    }
}
