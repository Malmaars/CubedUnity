using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class FloatAround : Node
    {
        bool running;
        Character character;
        public FloatAround(Character c) { character = c; }
        public override Result Run()
        {
            UpdateTracking(character);

            if (!running)
                character.interacting = true;
            
            //invoke the animation
            character.animator.Play("FloatAround");

            if(character.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
            {
                character.interacting = false;
                //the animation has finished, return the character to the default state
                character.animator.SetTrigger("Return");
                return Result.success;
            }

            return Result.running;
        }
    }
}