using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/ AudioPackage")]
[Serializable]
public class AudioPackage : ScriptableObject
{
    public List<AudioClip> audioClips;
    public float volume;
    //other sound things?
}
