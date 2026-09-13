using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ArenaParticles : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> particleSystems;

    private void OnEnable()
    {
        DiceProp.TutorialDicePickedUp += EnableParticles;
        DiceProp.TutorialDiceDropped += DisableParticles;

    }
    private void OnDisable()
    {
        DiceProp.TutorialDicePickedUp -= EnableParticles;
        DiceProp.TutorialDiceDropped -= DisableParticles;
    }

    private void EnableParticles()
    {
        foreach (ParticleSystem particle in particleSystems)
        {
           // particle.SetActive(true);
           particle.Play();
        }
    }

    private void DisableParticles()
    {
        foreach (ParticleSystem particle in particleSystems)
        {
          //  particle.SetActive(false);
          if (particle.isPlaying) { particle.Stop(); }
          
        }
    }
}
