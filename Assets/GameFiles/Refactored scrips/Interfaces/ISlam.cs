using UnityEngine;

public interface ISlam
{
    int slamDamage { get; set; }
    Color slamColour { get; set; }
    float chargeTime { get; set; }
    float slamRange { get; set; }
    Vector3 slamPositionOffset { get; set; }
    bool doesActionPreventMovement { get; set; }
}
