using System;
using UnityEngine;

[Serializable]
public class GeneratePerimeterPoints : BaseEntityMovement
{
    private ITarget target;
    float interval = 0.15f;
    float timer = 0f;

    public override void StartMovement(Entity ownerEntity)
    {
        base.StartMovement(ownerEntity);
        if (ownerEntity is ITarget target)
        { 
            this.target = target;
        }
    }

    public override void UpdateMovement()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            target.GeneratePerimeterPoints();
            timer = interval;
        }
    }

    public override BaseEntityMovement Clone()
    {
        return new GeneratePerimeterPoints();
    }
}
