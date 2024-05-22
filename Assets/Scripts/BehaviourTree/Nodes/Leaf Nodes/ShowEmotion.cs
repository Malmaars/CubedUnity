using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum emotion
{
    Happy,
    Angry,
    Delight,
    Dumb,
    Upset,
    Shy
}

public static class ShowEmotion
{
    static List<GameObject> openEmotionParticles = new List<GameObject>();
    static List<GameObject> closedEmotionParticles = new List<GameObject>();
    public static void StartEmotion(Character c, emotion toShow, float duration)
    {
        for (int i = 0; i < closedEmotionParticles.Count; i++)
        {
            if (!closedEmotionParticles[i].GetComponent<ParticleSystem>().isPlaying)
            {
                GameObject temp = closedEmotionParticles[i];
                closedEmotionParticles.Remove(temp);
                openEmotionParticles.Add(temp);
                i--;
            }
        }

        GameObject emotionParticleEffect;
        //start the particle effect
        if (openEmotionParticles.Count > 0)
        {
            //get an emotion particle from the open list
            emotionParticleEffect = openEmotionParticles[0];
            openEmotionParticles.Remove(emotionParticleEffect);
            emotionParticleEffect.transform.parent = c.actor.transform;
        }

        else
        {
            //make a new emotion particle
            emotionParticleEffect = Object.Instantiate(Resources.Load("Particles/EmotionParticles") as GameObject, c.actor.transform);
        }
        closedEmotionParticles.Add(emotionParticleEffect);
        emotionParticleEffect.transform.localPosition = new Vector3(0, 1.8f, 0);
        var particleMain = emotionParticleEffect.GetComponent<ParticleSystem>().main;
        particleMain.duration = duration;
        emotionParticleEffect.GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>().material = Resources.Load("Emotions/" + toShow.ToString()) as Material;
        emotionParticleEffect.GetComponent<ParticleSystem>().Play();

        //add the particle to the closed list until it's finished
    }
}