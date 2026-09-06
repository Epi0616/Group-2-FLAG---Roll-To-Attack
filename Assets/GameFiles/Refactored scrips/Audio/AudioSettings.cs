using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public AudioMixerGroup masterMixerGroup;
    public AudioMixerGroup sfxMixerGroup;
    public AudioMixerGroup musicMixerGroup;

    [SerializeField] InteractableMovingSlider master, sfx, music;

    public void AdjustMasterVolume()
    {
        float volume = master.value;
        float adjustedVolume = Mathf.Log10(volume) * 20;
        if (volume == 0)
        {
            adjustedVolume = -100f;
        }

        masterMixerGroup.audioMixer.SetFloat("Master Volume", adjustedVolume);
    }

    public void AdjustSFXVolume()
    {
        float volume = sfx.value;
        float adjustedVolume = Mathf.Log10(volume) * 20;
        if (volume == 0)
        {
            adjustedVolume = -100f;
        }

        masterMixerGroup.audioMixer.SetFloat("SoundFX Volume", adjustedVolume);
    }

    public void AdjustMusicVolume()
    {
        float volume = music.value;
        float adjustedVolume = Mathf.Log10(volume) * 20;
        if (volume == 0)
        {
            adjustedVolume = -100f;
        }

        masterMixerGroup.audioMixer.SetFloat("Music Volume", adjustedVolume);
    }
}

