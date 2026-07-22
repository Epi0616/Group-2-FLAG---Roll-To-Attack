using UnityEngine;

public interface IFireballAction
{
    GameObject fireballObj { get; set; }
    GameObject fireballRootBone { get; set; }
    int fireballDamage { get; set; }
    int fireFieldDamage { get; set; }
    LayerMask targetableLayers { get; set; }
}
