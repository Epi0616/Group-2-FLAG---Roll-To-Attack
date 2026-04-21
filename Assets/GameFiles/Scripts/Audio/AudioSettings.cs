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

    [SerializeField] Slider master;
    [SerializeField] Slider sfx;
    [SerializeField] Slider music;

    public void AdjustMasterVolume()
    {
        float volume = master.value;
        float adjustedVolume = Mathf.Log10(volume) * 20;
        masterMixerGroup.audioMixer.SetFloat("Master Volume", adjustedVolume);

        Debug.Log("adjusted vloume to: " + volume);
    }
}
