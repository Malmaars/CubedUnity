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
public class InventoryItem 
{
    public string name;
    public GameObject visual;
    public Animator animator;

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

    public void DisableItem()
    {
        visual.SetActive(false);
    }

    public void EnableItem()
    {
        visual.SetActive(true);
    }
}
