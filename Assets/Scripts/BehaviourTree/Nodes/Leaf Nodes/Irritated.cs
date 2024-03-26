using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

namespace BehaviourTree
{
    public class Irritated : Confused
    {
        public Irritated(Character c) : base (c){}

        protected override void Initialize(Character c)
        {
            character = c;
            particlesObject = Object.Instantiate(Resources.Load("Particles/IrritatedParticles") as GameObject);
            confusedParticles = particlesObject.GetComponent<ParticleSystem>();
            particlesObject.transform.parent = character.actor.transform;
            particlesObject.transform.localPosition = Vector3.up * 1.5f;
            particlesObject.transform.localScale = Vector3.one;
        }
    }
}