using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Waves/ Wave")]
public class WaveObj : ScriptableObject
{
    public Wave wave;

    public Wave Create()
    {
        return wave.Clone();
    }
}
