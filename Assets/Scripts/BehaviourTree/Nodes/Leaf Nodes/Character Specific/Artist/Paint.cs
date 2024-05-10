using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Paint : Node
    {
        Character character;
        float timer = 10;
        bool start;

        GameObject brush, painting;
        GameObject paintParticles;
        public Paint(Character c) 
        {
            //spawn painting items, and keep them in memory
            brush = Object.Instantiate(Resources.Load("Items/Artist/brush") as GameObject, c.actor.transform.Find("KayKit Animated Character/Body/armRight/handSlotRight"));
            brush.transform.localPosition = new Vector3(0.08f, 0, 0);
            brush.transform.localScale = Vector3.one * 4;
            brush.SetActive(false);
            
            painting = Object.Instantiate(Resources.Load("Items/Artist/easel_with_painting") as GameObject, c.actor.transform);
            painting.transform.localPosition = new Vector3(0, 0, 1.5f);
            painting.transform.localRotation = Quaternion.Euler(0, 180, 0);
            painting.transform.localScale = Vector3.one * 4;
            painting.SetActive(false);

            paintParticles = Object.Instantiate(Resources.Load("Particles/PaintParticles") as GameObject, c.actor.transform);
            paintParticles.transform.localPosition = new Vector3(0, 1, 0);

            character = c; 
        }

        //create a painting from several options
        public override Result Run()
        {
            UpdateTracking(character);

            if (!start)
            {
                //for flavor, set the character to a random rotation
                brush.SetActive(true);
                painting.SetActive(true);
                //play the animation
                character.animator.Play("Paint");
                paintParticles.GetComponent<ParticleSystem>().Play();

                character.interacting = true;
                timer = 10;
                start = true;
            }

            if(timer > 0)
            {
                timer -= Time.deltaTime;
                //continue playing the animation
                return Result.running;
            }

            else
            {
                //end the interaction
                brush.SetActive(false);
                painting.SetActive(false);
                paintParticles.GetComponent<ParticleSystem>().Stop();
                character.actor.transform.forward = Vector3.forward;
                character.animator.SetTrigger("Return");
                start = false;
                return Result.success;
            }
        }

        //function to make different paintings, perhaps of characters
        public void SetPaintingTexture()
        {

        }

    }
}