using UnityEngine;

public class GroundedCondition : BaseCondition
{
    private EnemyStateController enemy;

    public GroundedCondition(bool required, EnemyStateController enemy)
    {
        this.enemy = enemy;
        isRequired = required;
        name = "GroundedCondition";
    }
    public override void Initialize(Entity entity) { }
    public override void ConditionUpdate() { }

    public override void ResetCondition() { }

    public override bool IsConditionMet()
    {
        return (enemy.IsGrounded());
    }
}
