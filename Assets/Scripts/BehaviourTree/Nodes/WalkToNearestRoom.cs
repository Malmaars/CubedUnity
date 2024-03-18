using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class WalkToNearestRoom : Node
{

    Character character;
    public WalkToNearestRoom(Character _owner) { character = _owner; }
    public override Result Run()
    {
        if (character == null)
            return Result.failed;

        Vector3 dest = new Vector3(character.currentRoom.visual.transform.position.x, character.currentRoom.visual.transform.position.y - 0.48f, character.currentRoom.visual.transform.position.z + 0.4f);


        //the destination should always be above, to the right, to the left, or below the character. Never multiple
        if (dest.x < character.actor.transform.position.x)
        {
            //face and walk left
            character.actor.transform.forward = Vector3.left;
            character.animator.SetBool("Walking", true);
            character.actor.transform.position += Vector3.ClampMagnitude((dest - character.actor.transform.position).normalized * Time.deltaTime * 0.5f, Vector3.Distance(dest, character.actor.transform.position));
        }

        else if (dest.x > character.actor.transform.position.x)
        {
            //face and walk right
            character.actor.transform.forward = Vector3.right;
            character.animator.SetBool("Walking", true);
            character.actor.transform.position += Vector3.ClampMagnitude((dest - character.actor.transform.position).normalized * Time.deltaTime * 0.5f, Vector3.Distance(dest, character.actor.transform.position));
        }

        if (dest.y > character.actor.transform.position.y)
        {
            //jump up
            character.animator.SetBool("Jump", true);
            character.actor.transform.position += Vector3.ClampMagnitude((dest - character.actor.transform.position).normalized * Time.deltaTime * 2f, Vector3.Distance(dest, character.actor.transform.position));
        }

        else if (dest.y < character.actor.transform.position.y)
        {
            //jump down
            character.animator.SetBool("Jump", true);
            character.actor.transform.position += Vector3.ClampMagnitude((dest - character.actor.transform.position).normalized * Time.deltaTime * 2f, Vector3.Distance(dest, character.actor.transform.position));
        }

        if (Vector3.Distance(character.actor.transform.position, dest) < 0.01f)
        {
            character.animator.SetBool("Walking", false);
            character.animator.SetBool("Jump", false);

            //pause a second until the next step?
            character.actor.transform.position = dest;
            character.actor.transform.forward = Vector3.forward;
            return Result.success;
        }

        return Result.running;
    }
}
