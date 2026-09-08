using UnityEngine;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour, ILoadPlayerPrefs
{
    public AudioMixerGroup masterMixerGroup;
    public AudioMixerGroup sfxMixerGroup;
    public AudioMixerGroup musicMixerGroup;

    [SerializeField] InteractableMovingSlider master, sfx, music;

    public void Start()
    {
        TryLoadPrefs();
    }

    public void AdjustMasterVolume()
    {
        float masterDB = ConvertToDecibels(master.value);
        masterMixerGroup.audioMixer.SetFloat("Master Volume", masterDB);
    }

    public void AdjustSFXVolume()
    {
        float sfxDB = ConvertToDecibels(sfx.value);
        masterMixerGroup.audioMixer.SetFloat("SoundFX Volume", sfxDB);
    }

    public void AdjustMusicVolume()
    {
        float musicDB = ConvertToDecibels(music.value);
        masterMixerGroup.audioMixer.SetFloat("Music Volume", musicDB);
    }

    public void TryLoadPrefs()
    {
        float master = PlayerPrefsManager.instance.GetFloat(PlayerValues.MasterVolumer);
        float music = PlayerPrefsManager.instance.GetFloat(PlayerValues.MusicVolume);
        float sfx = PlayerPrefsManager.instance.GetFloat(PlayerValues.SFXVolume);

        float masterDB = ConvertToDecibels(master);
        float musicDB = ConvertToDecibels(music);
        float sfxDB = ConvertToDecibels(sfx);

        masterMixerGroup.audioMixer.SetFloat("Master Volume", masterDB);
        musicMixerGroup.audioMixer.SetFloat("Music Volume", musicDB);
        masterMixerGroup.audioMixer.SetFloat("SoundFX Volume", sfxDB);

        this.master.value = master;
        this.music.value = music;
        this.sfx.value = sfx;
    }

    private float ConvertToDecibels(float volume)
    {
        return volume > 0 ? Mathf.Log10(volume) * 20 : -80f;
    }
}

