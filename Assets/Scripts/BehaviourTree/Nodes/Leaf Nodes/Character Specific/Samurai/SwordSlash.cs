using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class SwordSlash : Node
    {
        Character character;
        bool start;

        GameObject sword;
        public SwordSlash(Character c)
        {
            character = c;
            sword = Object.Instantiate(Resources.Load("Items/Samurai/Sword") as GameObject, c.actor.transform.Find("KayKit Animated Character/Body/armRight/handSlotRight"));
            sword.transform.localScale = Vector3.one * 4;
            sword.SetActive(false);
        }
        public override Result Run()
        {
            if (!start)
            {
                sword.SetActive(true);
                character.animator.Play("SwordPrepare");
                start = true;
            }

            if (character.animator.GetCurrentAnimatorStateInfo(0).IsName("Finished"))
            {
                Debug.Log("Swordslash is finished");
                //the animation is finished
                start = false;
                sword.SetActive(false);
                return Result.success;
            }

            return Result.running;
        }

        public void ReAssign(Character c)
        {
            character = c;
            sword.transform.parent = c.actor.transform.Find("KayKit Animated Character/Body/armRight/handSlotRight");
            sword.transform.localScale = Vector3.one * 4;
            sword.SetActive(false);
        }
    }
}