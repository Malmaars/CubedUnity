using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter : IWalkable, INeedy
{
    public GameObject actor { get; set; }
    public Animator animator { get; set; }
}
