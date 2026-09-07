using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource soundObject;

    private Dictionary<GameObject, LoopingClip> loopingObjects;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        loopingObjects = new Dictionary<GameObject, LoopingClip>();
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

    public void PlaySingleLoopingClip(GameObject owner, AudioPackage audioPackage, Vector3 position = default)
    {
        if (loopingObjects.ContainsKey(owner))
        {
            if (loopingObjects[owner].loopingClips.ContainsKey(audioPackage))
            {
                return;
            }
        }
        else
        {
            loopingObjects.Add(owner, new LoopingClip());
        }

        if (audioPackage == null) return;
        List<AudioClip> audioClips = audioPackage.audioClips;

        if (audioClips.Count <= 0) return;
        int randomIndex = Random.Range(0, audioClips.Count);

        AudioClip chosenClip = audioClips[randomIndex];
        if (chosenClip == null) return;

        AudioSource audioSource = ObjectPoolManager.SpawnObject(soundObject, position, Quaternion.identity);

        loopingObjects[owner].loopingClips.Add(audioPackage, audioSource);
        audioSource.clip = chosenClip;
        audioSource.volume = audioPackage.volume;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopSingleLoopingClip(GameObject owner, AudioPackage audioPackage)
    {
        if (!loopingObjects.ContainsKey(owner)) return;
        if (audioPackage == null || audioPackage.audioClips.Count <= 0) return;

        if (!loopingObjects[owner].loopingClips.ContainsKey(audioPackage)) return;

        AudioSource audioSource = loopingObjects[owner].loopingClips[audioPackage];
        audioSource.loop = false;
        loopingObjects[owner].loopingClips.Remove(audioPackage);

        ObjectPoolManager.ReturnObjectToPool(audioSource.gameObject);
    }
}

public class LoopingClip
{
    public Dictionary<AudioPackage, AudioSource> loopingClips;

    public LoopingClip()
    {
        loopingClips = new Dictionary<AudioPackage, AudioSource>();
    }
}
