using UnityEngine;

public interface IRadialProjectile
{
    GameObject radialObj { get; set; }
    LayerMask radialTargetableLayers { get; }
}
