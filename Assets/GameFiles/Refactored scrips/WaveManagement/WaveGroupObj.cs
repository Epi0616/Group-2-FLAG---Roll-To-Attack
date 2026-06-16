using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Waves/ WaveGroup")]
public class WaveGroupObj : ScriptableObject
{
    public WaveGroup waveGroup;

    public WaveGroup Create()
    {
        return waveGroup.Clone();
    }
}
