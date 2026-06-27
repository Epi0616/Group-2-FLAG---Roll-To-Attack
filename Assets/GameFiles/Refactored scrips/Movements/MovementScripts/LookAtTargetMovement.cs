using UnityEngine;
using System;

[Serializable]
public class LookAtTargetMovement : BaseEntityMovement
{
    private Vector3 targetDir;
    private Quaternion lookRotation;
    public float rotationSpeed = 10f;

    public LookAtTargetMovement() { }
    public LookAtTargetMovement(float rotationSpeed)
    {
        this.rotationSpeed = rotationSpeed;
    }

    public override void UpdateMovement()
    {
        //Debug.Log("Lookin");
        if (ownerEntity == null) return;
        if (ownerEntity.target == null) return;
        targetDir = ownerEntity.target.transform.position - ownerEntity.transform.position;
        targetDir.y = 0f;
        if (targetDir == Vector3.zero) { return; }
        lookRotation = Quaternion.LookRotation(targetDir);
        
        ownerEntity.transform.rotation = Quaternion.Slerp(ownerEntity.transform.rotation, lookRotation, 10f * Time.deltaTime);
    }

    public override BaseEntityMovement Clone()
    {
        return new LookAtTargetMovement(rotationSpeed);
    }
}
