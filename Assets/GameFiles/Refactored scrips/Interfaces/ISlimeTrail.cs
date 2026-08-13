using UnityEngine;

public interface ISlimeTrail
{
    GameObject slimeFieldObj { get; set; }
    LayerMask slimeableMask { get; set; }
    bool isCharging { get; set; }
}
