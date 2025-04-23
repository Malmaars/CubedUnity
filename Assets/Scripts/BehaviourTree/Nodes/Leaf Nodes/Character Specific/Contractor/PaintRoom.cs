using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class PaintRoom : Node
    {
        Character character;
        bool running;
        float timer = 8;

        GameObject particlesObject;

        public PaintRoom(Character c) 
        { 
            character = c;
            particlesObject = Object.Instantiate(Resources.Load("Particles/PaintRoomParticles") as GameObject);
        }
        public override Result Run()
        {
            UpdateTracking(character);
            //play animation
            if (!running)
            {
                particlesObject.transform.parent = character.currentRoom.visual.transform;
                particlesObject.transform.localPosition = Vector3.forward * 0.5f;
                particlesObject.transform.localScale = Vector3.one;

                character.actor.transform.forward = -Vector3.forward;
                character.currentRoom.occupied = true;
                character.interacting = true;
                character.animator.Play("Build");

                particlesObject.GetComponent<ParticleSystem>().Play();
                timer = 10;
                running = true;
            }

            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                foreach (Material m in character.currentRoom.visual.transform.Find("Shell").GetChild(0).GetComponent<MeshRenderer>().materials)
                {
                    m.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                }

                character.actor.transform.forward = Vector3.forward;
                character.currentRoom.occupied = false;
                character.interacting = false;
                character.animator.SetTrigger("Return");
                particlesObject.GetComponent<ParticleSystem>().Stop();
                running = false;

                return Result.success;
            }
            return Result.running;
        }
    }
}