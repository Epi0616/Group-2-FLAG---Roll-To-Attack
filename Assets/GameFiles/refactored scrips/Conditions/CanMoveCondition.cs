using UnityEngine;
using System;

[Serializable]
public class CanMoveCondition : BaseCondition
{
    private IMoveable move;
    private bool valid = true;

    public CanMoveCondition() { }
    public CanMoveCondition(bool inverse)
    {
        this.inverse = inverse;
    }
    public override void Initialize(Entity entity)
    {
        move = entity as IMoveable;
        valid = true;
        if (move == null) { valid = false; } 
    }
    public override void ConditionUpdate()
    {
    }
    public override void ResetCondition()
    {
    }
    public override bool IsConditionMet()
    {
        if (valid && !inverse)
        {
            return move.canMove;
        }
        else if (valid && inverse)
        {
            return !move.canMove;
        }
        return false;
    }
    public override BaseCondition Clone()
    {
        return new CanMoveCondition(inverse);
    }
}
