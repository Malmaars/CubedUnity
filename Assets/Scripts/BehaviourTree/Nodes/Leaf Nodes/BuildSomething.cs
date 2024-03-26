using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class BuildSomething : Node
    {
        Character character;

        ParticleSystem MakingClouds;
        GameObject particlesObject;

        float timer = 5;
        bool finalAnimation;

        InventoryItem item;

        //since this is a node for a cube, it needs to be able to be passed on to different characters. So the character needs to be able to be reassigned

        public override Result Run()
        {
            if (MakingClouds.isPlaying)
                return Result.running;

            //if we reach here, the particle system is done playing

            //play a final animation
            if (timer > 0)
            {
                character.animator.SetBool("Build", false);

                //show acquired item
                item = new InventoryItem(Resources.Load("Items/stool") as GameObject);
                item.visual.transform.position = character.transform.position + Vector3.right * -0.85f + Vector3.up * 1.25f;
                item.animator.SetBool("Spin", true);
                timer -= Time.deltaTime;
                return Result.running;
            }

            else
            {
                timer = 5;
                item.animator.SetBool("Spin", false);
                character.AddItemToInventory(item);
                character.currentRoom.occupied = false;
                character.interacting = false;
                character.animator.SetTrigger("Return");
                return Result.success;
            }
        }

        public void Initialize(Character c)
        {
            character = c;

            if (particlesObject == null)
            {
                particlesObject = Object.Instantiate(Resources.Load("Particles/MakingClouds") as GameObject);
                MakingClouds = particlesObject.GetComponent<ParticleSystem>();
            }

            particlesObject.transform.parent = character.actor.transform;
            particlesObject.transform.localPosition = Vector3.zero;
            particlesObject.transform.localScale = Vector3.one;

            character.actor.transform.forward = -Vector3.forward;

            character.currentRoom.occupied = true;
            character.interacting = true;
            character.animator.SetBool("Build", true);
            MakingClouds.Play();
        }
    }
}
