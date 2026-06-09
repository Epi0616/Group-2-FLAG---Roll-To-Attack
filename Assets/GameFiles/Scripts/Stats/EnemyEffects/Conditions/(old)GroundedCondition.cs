using UnityEngine;

public class oldGroundedCondition : BaseCondition
{
    private EnemyStateController enemy;

    public oldGroundedCondition(EnemyStateController enemy)
    {
        this.enemy = enemy;
        //isRequired = required;
        name = "GroundedCondition";
    }
    public override void Initialize(Entity entity) { }
    public override void ConditionUpdate() { }

    public override void ResetCondition() { }

    public override bool IsConditionMet()
    {
        return (enemy.IsGrounded());
    }
    public override BaseCondition Clone()
    {
        return new oldGroundedCondition(enemy);
    }
}
