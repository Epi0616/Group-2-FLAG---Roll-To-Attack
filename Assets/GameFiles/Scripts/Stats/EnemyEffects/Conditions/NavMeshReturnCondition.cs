using UnityEngine;
using UnityEngine.AI;

public class NavMeshReturnCondition : BaseCondition
{
    private EnemyStateController enemy;
    private float distance = 100.0f;

    public NavMeshReturnCondition(bool required, EnemyStateController enemy)
    {
        this.enemy = enemy;   
        isRequired = required;
        name = "NavReturnCondition";
    }
    public override void Initialize(Entity entity)
    {
        //this.entity = entity;
    }
    public override void ConditionUpdate()
    {
      
        if (enemy.rb.linearVelocity.magnitude <= 2f)
        {
            NavMeshHit hit;
            bool validNavMeshNode = NavMesh.SamplePosition(enemy.transform.position, out hit, 5f, NavMesh.AllAreas);
            if (validNavMeshNode)
            {
                Vector3 destinationPos = new Vector3(hit.position.x, hit.position.y, hit.position.z);
                Vector3 returntoNavMeshDirection = (destinationPos - enemy.transform.position).normalized;
                //returntoNavMeshDirection.y = enemy.transform.position.y;
                enemy.rb.MovePosition(enemy.transform.position + returntoNavMeshDirection * 10f * Time.deltaTime);

                distance = Vector3.Distance(enemy.transform.position, destinationPos);

            }
        }
    }

    public override void ResetCondition() { }

    public override bool IsConditionMet()
    {        
        return (distance < 0.2f);
    }
    public override BaseCondition Clone()
    {
        return new NavMeshReturnCondition(isRequired, enemy);
    }
}
