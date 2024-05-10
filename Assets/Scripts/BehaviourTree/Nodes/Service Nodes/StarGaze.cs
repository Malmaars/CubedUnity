using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class StarGaze : Node
    {
        bool running; 
        IServicable servicable;
        GameObject solarSystem, spaceParticles;

        float timer;
        public StarGaze(IServicable i, Cube cb) 
        { 
            servicable = i;
            solarSystem = Object.Instantiate(Resources.Load("Decoratives/SolarSystem") as GameObject, cb.visual.transform.Find("Environment"));
            solarSystem.transform.localPosition = Vector3.zero;
            solarSystem.SetActive(false);

            spaceParticles = Object.Instantiate(Resources.Load("Particles/SpaceParticles") as GameObject, cb.visual.transform.Find("Environment"));
        }
        public override Result Run()
        {
            if (servicable.asker == null)
                return Result.failed;

            if (!running)
            {
                solarSystem.SetActive(true);
                spaceParticles.GetComponent<ParticleSystem>().Play();
                timer = 20f;
                running = true;

                servicable.asker.actor.transform.forward = -Vector3.forward;
                servicable.asker.animator.Play("LookUp");
                servicable.asker.currentRoom.occupied = true;
                servicable.asker.interacting = true;
            }

            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                running = false;
                solarSystem.SetActive(false);
                servicable.asker.actor.transform.forward = Vector3.forward;
                servicable.asker.animator.SetTrigger("Return");
                servicable.asker.currentRoom.occupied = false;
                servicable.asker.interacting = false;
                return Result.success;
            }


            return Result.running;
        }
    }
}