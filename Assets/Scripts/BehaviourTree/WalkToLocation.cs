using UnityEngine;

namespace BehaviourTree
{
    //a static general use class for making a character walk to a location (vector3).
    //
    //!!Should only be used to let characters walk to specific locations outside of the cube system.!!
    public static class WalkToLocation
    {
        public static bool WalkCharacter(Character character, Vector3 newPos)
        {
            Vector3 dest = newPos;

            //the destination should always be above, to the right, to the left, or below the character. Never multiple
            //Haha just kidding, now it can. Give a preference to one over the other

            if (dest.x < character.actor.transform.position.x)
            {
                dest = new Vector3(dest.x, character.actor.transform.position.y, dest.z);
                //face and walk left
                character.actor.transform.forward = Vector3.left;
                character.animator.SetBool("Walking", true);
                character.actor.transform.position += Vector3.ClampMagnitude((dest - character.actor.transform.position).normalized * Time.deltaTime * 0.5f, Vector3.Distance(dest, character.actor.transform.position));
            }

            else if (dest.x > character.actor.transform.position.x)
            {
                dest = new Vector3(dest.x, character.transform.position.y, dest.z);
                //face and walk right
                character.actor.transform.forward = Vector3.right;
                character.animator.SetBool("Walking", true);
                character.actor.transform.position += Vector3.ClampMagnitude((dest - character.actor.transform.position).normalized * Time.deltaTime * 0.5f, Vector3.Distance(dest, character.actor.transform.position));
            }

            else if (dest.y > character.actor.transform.position.y)
            {
                dest = new Vector3(character.actor.transform.position.x, dest.y, dest.z);
                //jump up
                character.animator.SetBool("Jump", true);
                character.actor.transform.position += Vector3.ClampMagnitude((dest - character.actor.transform.position).normalized * Time.deltaTime * 2f, Vector3.Distance(dest, character.actor.transform.position));
            }

            else if (dest.y < character.actor.transform.position.y)
            {
                dest = new Vector3(character.actor.transform.position.x, dest.y, dest.z);
                //jump down
                character.animator.SetBool("Jump", true);
                character.actor.transform.position += Vector3.ClampMagnitude((dest - character.actor.transform.position).normalized * Time.deltaTime * 2f, Vector3.Distance(dest, character.actor.transform.position));
            }

            if (Vector3.Distance(character.actor.transform.position, newPos) < 0.01f)
            {
                character.animator.SetBool("Walking", false);
                character.animator.SetBool("Jump", false);

                //pause a second until the next step?
                character.actor.transform.position = newPos;
                character.actor.transform.forward = Vector3.forward;
                return true;
            }

            return false;
        }
    }
}