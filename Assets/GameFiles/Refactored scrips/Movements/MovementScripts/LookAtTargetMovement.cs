using UnityEngine;
using System;

[Serializable]
public class LookAtTargetMovement : BaseEntityMovement
{
    private Vector3 targetDir;
    private Quaternion lookRotation;
    public override void UpdateMovement()
    {
        //Debug.Log("Lookin");
        if (ownerEntity == null) return;
        if (ownerEntity.target == null) return;
        targetDir = ownerEntity.target.transform.position - ownerEntity.transform.position;
        targetDir.y = ownerEntity.transform.position.y;
        lookRotation = Quaternion.LookRotation(targetDir);
        lookRotation.z = 0f;
        lookRotation.x = 0f;
        // float t = activeTimer / duration;
        
        ownerEntity.transform.rotation = Quaternion.Slerp(ownerEntity.transform.rotation, lookRotation, Time.deltaTime);
    }

    public override BaseEntityMovement Clone()
    {
        return new LookAtTargetMovement();
    }
}
