using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class BuildSomething : Node
    {
        IServicable servicable;

        ParticleSystem MakingClouds;
        GameObject particlesObject;

        GameObject[] AllObjects;

        float timer = 8;
        bool start = false;

        InventoryItem item;

        public BuildSomething(IServicable _servicable)
        {
            Initialize(_servicable);
            AllObjects = Resources.LoadAll("Items/Buildable", typeof(GameObject)).Cast<GameObject>().ToArray();
            Debug.Log(AllObjects.Length);
        }

        //since this is a node for a cube, it needs to be able to be passed on to different characters. So the character needs to be able to be reassigned

        public override Result Run()
        {
            UpdateTracking(servicable.asker);

            if (!start)
            {
                particlesObject.transform.parent = servicable.asker.actor.transform;
                particlesObject.transform.localPosition = Vector3.up * 2;
                particlesObject.transform.localScale = Vector3.one;


                GameObject randomItem = AllObjects[Random.Range(0, AllObjects.Length)];
                item = new InventoryItem(randomItem);
                item.visual.transform.position = servicable.asker.actor.transform.position - Vector3.right * 0.25f + Vector3.up * 0.28f;
                item.visual.transform.parent = servicable.asker.actor.transform;

                servicable.asker.actor.transform.forward = -Vector3.forward;

                servicable.asker.currentRoom.occupied = true;
                servicable.asker.interacting = true;
                servicable.asker.animator.SetBool("Build", true);
                MakingClouds.Play();


                item.DisableItem();

                start = true;
            }

            if (MakingClouds.isPlaying)
                return Result.running;

            //if we reach here, the particle system is done playing


            //play a final animation
            if (timer > 0)
            {
                servicable.asker.animator.SetBool("Build", false);
                servicable.asker.actor.transform.forward = Vector3.forward;

                //show acquired item
                item.EnableItem();
                item.animator.SetBool("Spin", true);
                timer -= Time.deltaTime;
                return Result.running;
            }

            else
            {
                timer = 8;
                item.animator.SetBool("Spin", false);
                servicable.asker.AddItemToInventory(item);
                servicable.asker.currentRoom.occupied = false;
                servicable.asker.animator.SetTrigger("Return");
                start = false;
                return Result.success;
            }
        }

        public void Initialize(IServicable _servicable)
        {
            servicable = _servicable;

            if (particlesObject == null)
            {
                particlesObject = Object.Instantiate(Resources.Load("Particles/BuildParticles") as GameObject);
                MakingClouds = particlesObject.GetComponent<ParticleSystem>();
            }
        }
    }
}
