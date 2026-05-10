using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource soundObject;

    private Dictionary<AudioClip, AudioSource> loopingClips;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        loopingClips = new Dictionary<AudioClip, AudioSource>();
    }

    public void PlaySoundClip(AudioClip audioClip, Vector3 position = default, float volume = 1f)
    {
        if (audioClip == null) return;

        AudioSource audioSource = ObjectPoolManager.SpawnObject(soundObject, position, Quaternion.identity);
        //Debug.Log(volume);
        audioSource.volume = 1f;
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        float clipLength = audioSource.clip.length;
        ObjectPoolManager.ReturnObjectToPool(audioSource.gameObject, clipLength);
    }

    public void PlayRandomSoundClip(AudioClip[] audioClips, Vector3 position = default, float volume = 1f)
    {
        if (audioClips == null) return;

        if (audioClips.Length <= 0) return;
        int randomIndex = Random.Range(0, audioClips.Length);

        if (audioClips[randomIndex] == null) return;
        PlaySoundClip(audioClips[randomIndex], position, volume);
    }

    public void PlaySingleLoopingClip(AudioClip audioClip, Vector3 position = default, float volume = 1f)
    {
        if (audioClip == null) return;
        if (loopingClips.ContainsKey(audioClip))
        {
            //Debug.Log("contains key"); 
            return;
        } 

        AudioSource audioSource = ObjectPoolManager.SpawnObject(soundObject, position, Quaternion.identity);

        loopingClips.Add(audioClip, audioSource);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopSingleLoopingClip(AudioClip audioClip)
    {
        if (audioClip == null) return;
        AudioSource audioSource = loopingClips[audioClip];
        audioSource.loop = false;
        loopingClips.Remove(audioClip);

        ObjectPoolManager.ReturnObjectToPool(audioSource.gameObject);
    }
}
