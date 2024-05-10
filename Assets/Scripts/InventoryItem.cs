using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "ScriptableObjects/InventoryItem")]

public class ScriptableItem : ScriptableObject
{
    public string itemName;
    public GameObject visual;
}

[System.Serializable]
public class InventoryItem : IServicable
{
    //Iservicable data
    public Character asker { get; set; }
    public Service[] services { get; set; }

    public bool beingUsed { get; set; }

    public string name;
    public GameObject visual;
    public Animator animator;

    //I don't yet know if it's better to make presets for known items, perhaps if different scripts want to spawn the same item
    public InventoryItem(GameObject _visual, string _name)
    {
        CreateItem(_visual);
        name = _name;
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
