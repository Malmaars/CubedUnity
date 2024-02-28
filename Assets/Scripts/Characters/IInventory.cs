using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventory
{
    protected List<Item> inventory { get; set; }

    public void AddItemToInventory(Item _toAdd);
}
