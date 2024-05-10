using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class SleepOnItem : Node
    {
        Character character;
        InventoryItem item;
        bool running, finishing;

        float timer;

        GameObject sleepParticles;

        public SleepOnItem(InventoryItem i)
        {
            item = i;
            sleepParticles = Object.Instantiate(Resources.Load("Particles/SleepParticles") as GameObject, item.visual.transform);
            sleepParticles.transform.localPosition = new Vector3(0, 1, -0.75f);
            sleepParticles.SetActive(false);
            timer = 20f;
        }

        public override Result Run()
        {
            if (!running)
            {
                if (item.asker == null)
                    return Result.failed;

                character = item.asker;

                item.EnableItem();
                item.visual.transform.parent = character.currentRoom.visual.transform.Find("Environment");
                item.visual.transform.localPosition = new Vector3(0, -0.48f, 0.35f);
                item.visual.transform.localRotation = Quaternion.Euler(0, -90f, 0);
                item.visual.transform.localScale = Vector3.one * 0.25f;

                character = item.asker;
                character.actor.transform.localPosition = new Vector3(-0.15f, -0.35f, 0.4f);
                character.actor.transform.localRotation = Quaternion.Euler(0, -90f, 0);

                character.animator.Play("LieDown");

                //play sleep particles
                sleepParticles.SetActive(true);
                sleepParticles.GetComponent<ParticleSystem>().Play();

                character.currentRoom.TurnOffLights();
                character.currentRoom.occupied = true;

                timer = 20f;
                running = true;
            }

            timer -= Time.deltaTime;

            if(timer <= 0 && !finishing)
            {
                //end sleep. wake up
                character.animator.Play("GetUp");
                finishing = true;
            }

            if (finishing)
            {
                if (character.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
                {
                    character.actor.transform.localPosition = new Vector3(0, -0.48f, 0.4f);
                    character.actor.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    item.DisableItem();
                    sleepParticles.GetComponent<ParticleSystem>().Stop();
                    sleepParticles.SetActive(false);
                    character.currentRoom.TurnOnLights();
                    character.currentRoom.occupied = false;
                    running = false;
                    finishing = false;
                    return Result.success;
                }
            }

            return Result.running;
        }
    }
}