using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "ScriptableObjects/InventoryItem")]

public class ScriptableItem : ScriptableObject
{
    public string itemName;
    public GameObject visual;
}
public class InventoryItem 
{
    public GameObject visual;
    public Animator animator;

    public InventoryItem(GameObject _visual)
    {
        CreateItem(_visual);
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
}
