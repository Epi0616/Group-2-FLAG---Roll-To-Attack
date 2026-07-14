using UnityEngine;

public interface IFireballAction
{
    GameObject fireballObj { get; set; }
    int fireballDamage { get; set; }
    int fireFieldDamage { get; set; }
    LayerMask targetableLayers { get; set; }
}
