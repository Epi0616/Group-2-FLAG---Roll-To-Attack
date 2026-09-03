using UnityEngine;
using System.Collections.Generic;

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

    public void PlaySound(AudioPackage audioPackage, Vector3 position = default)
    {
        if (audioPackage == null) return;
        List<AudioClip> audioClips = audioPackage.audioClips;

        if (audioClips.Count <= 0) return;
        int randomIndex = Random.Range(0, audioClips.Count);

        if (audioClips[randomIndex] == null) return;
        PlayAudioClip(audioClips[randomIndex], position, audioPackage.volume);
    }

    private void PlayAudioClip(AudioClip audioClip, Vector3 position = default, float volume = 1f)
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

    //public void PlaySingleLoopingClip(AudioPackage audioPackage, Vector3 position = default, float volume = 1f)
    //{
    //    if (audioPackage == null) return;
    //    if (audioClip == null) return;
    //    if (loopingClips.ContainsKey(audioClip))
    //    {
    //        //Debug.Log("contains key"); 
    //        return;
    //    } 

    //    AudioSource audioSource = ObjectPoolManager.SpawnObject(soundObject, position, Quaternion.identity);

    //    loopingClips.Add(audioClip, audioSource);
    //    audioSource.clip = audioClip;
    //    audioSource.volume = volume;
    //    audioSource.loop = true;
    //    audioSource.Play();
    //}

    //public void StopSingleLoopingClip(AudioClip audioClip)
    //{
    //    if (audioClip == null) return;
    //    if (!loopingClips.ContainsKey(audioClip)) return;

    //    AudioSource audioSource = loopingClips[audioClip];
    //    audioSource.loop = false;
    //    loopingClips.Remove(audioClip);

    //    ObjectPoolManager.ReturnObjectToPool(audioSource.gameObject);
    //}
}
