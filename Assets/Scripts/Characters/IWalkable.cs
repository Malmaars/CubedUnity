using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWalkable
{
    public Cube currentRoom { get; set; }

    public Cube RequestRoom();
}
