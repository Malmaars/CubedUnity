using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

namespace BehaviourTree
{
    public class Confused : Node
    {
        protected Character character;

        protected ParticleSystem confusedParticles;
        protected GameObject particlesObject;

        protected float timer = 2;

        public Confused(Character c)
        {
            Initialize(c);
        }
        public override Result Run()
        {
            UpdateTracking(character);

            if (timer == 2)
            {
                character.interacting = true;
                confusedParticles.Play();
            }

            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                timer = 2;
                confusedParticles.Stop();
                character.confused = false;
                character.interacting = false;
                return Result.success;
            }

            return Result.running;
        }

        protected virtual void Initialize(Character c)
        {
            character = c;
            particlesObject = Object.Instantiate(Resources.Load("Particles/ConfusedParticles") as GameObject);
            confusedParticles = particlesObject.GetComponent<ParticleSystem>();
            particlesObject.transform.parent = character.actor.transform;
            particlesObject.transform.localPosition = Vector3.up * 1.5f;
            particlesObject.transform.localScale = Vector3.one;
        }
    }
}