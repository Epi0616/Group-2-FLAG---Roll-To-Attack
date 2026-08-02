using System;
using UnityEngine;

[Serializable]
public class BossEscapeRotate : NavMeshEscapeMovement
{
    public BossEscapeRotate() { }

    public override BaseEntityMovement Clone()
    {
        return new BossEscapeRotate();
    }
}
