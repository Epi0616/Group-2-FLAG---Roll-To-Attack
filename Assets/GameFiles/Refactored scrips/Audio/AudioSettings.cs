using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour, ILoadPlayerPrefs
{
    public AudioMixerGroup masterMixerGroup;
    public AudioMixerGroup sfxMixerGroup;
    public AudioMixerGroup musicMixerGroup;

    [SerializeField] InteractableMovingSlider master, sfx, music;

    private void OnEnable()
    {
        TryLoadPrefs();
    }

    public void Start()
    {
        TryLoadPrefs();
    }

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

    public void TryLoadPrefs()
    {
        float master = PlayerPrefsManager.instance.GetFloat(PlayerValues.MasterVolumer);
        float music = PlayerPrefsManager.instance.GetFloat(PlayerValues.MusicVolume);
        float sfx = PlayerPrefsManager.instance.GetFloat(PlayerValues.SFXVolume);

        float adjustedVolume = Mathf.Log10(master) * 20;
        masterMixerGroup.audioMixer.SetFloat("Master Volume", adjustedVolume);

        adjustedVolume = Mathf.Log10(music) * 20;
        masterMixerGroup.audioMixer.SetFloat("Music Volume", adjustedVolume);

        adjustedVolume = Mathf.Log10(sfx) * 20;
        masterMixerGroup.audioMixer.SetFloat("SoundFX Volume", adjustedVolume);
    }
}

