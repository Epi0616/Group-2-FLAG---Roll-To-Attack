using UnityEngine;

public class ParticleEffectInstance : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private bool canColourOverride;
    
    public void PlayParticleEffect(EffectSettings settings)
    {
        if (canColourOverride)
        {
            var main = particleSystem.main;
            main.startColor = settings.overrideColour; 
        }
        
        particleSystem.Play();
    }

    
}
