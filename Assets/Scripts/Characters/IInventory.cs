using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventory
{
    public List<InventoryItem> inventory { get; set; }

    public void AddItemToInventory(InventoryItem _toAdd);
}
