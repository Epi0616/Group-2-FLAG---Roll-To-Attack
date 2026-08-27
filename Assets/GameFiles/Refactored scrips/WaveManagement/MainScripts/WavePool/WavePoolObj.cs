using UnityEngine;

[CreateAssetMenu(menuName = "Waves/ WavePool")]
public class WavePoolObj : ScriptableObject
{
    public WavePool wavePool;

    public WavePool Create()
    {
        return wavePool.Clone();
    }
}
