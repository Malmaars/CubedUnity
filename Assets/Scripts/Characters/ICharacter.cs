using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter : IWalkable, IServicable
{
    public GameObject actor { get; set; }
    public Animator animator { get; set; }
}
