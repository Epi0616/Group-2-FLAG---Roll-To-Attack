using UnityEngine;

public class EnemySpawnState : EnemyBaseState
{
    

    public override void EnterState(EnemyStateController enemy)
    {
        base.EnterState(enemy);

        enemy.isSpawning = true;

        if (enemy.hasSpawnVibration)
        {
            if (enemy.animator != null)
            {
                enemy.animator.speed = 0f;
            }
            //enemy.StartSpawnVibration();
        }
        else
        {
            enemy.isSpawning = false;
            enemy.ChangeState(new EnemyMoveState());
        }
    }

    public override void ExitState()
    {
        enemy.isSpawning = false;
        if (enemy.animator != null)
        {
            enemy.animator.speed = 1f;
        }
    }

}

public class VibratingSpawnState : EnemySpawnState
{
    private float timeElapsed;
    private Vector3 startPos;
    private Vector3 endPos;
     
    public override void EnterState(EnemyStateController enemy)
    {
        base.EnterState(enemy);

        enemy.enemyAgent.updatePosition = false;
        enemy.enemyAgent.updateRotation = false;
        enemy.enemyAgent.enabled = false;

        timeElapsed = 0f;

        startPos = enemy.transform.position;
        endPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 9.5f, enemy.transform.position.z);
        
    }
    public override void UpdateState()
    {
        if (timeElapsed > 5f)
        {
            
            Debug.Log("Spawning Ended");
            enemy.StopVibrating();
            enemy.transform.position = endPos;
            enemy.isSpawning = false;
            enemy.ChangeState(new EnemyMoveState());

        }

        Vector3 lerpOffset = Vector3.Lerp(startPos, endPos, timeElapsed / 5f);
        enemy.transform.position = lerpOffset + SpawningAnimationVibrateOffset();
        timeElapsed += Time.deltaTime;
        
    }

    private Vector3 SpawningAnimationVibrateOffset()
    {
        float x = Mathf.Sin(Time.time * enemy.vibrateSpeed) * enemy.vibrateIntensity;
        float z = Mathf.Sin(Time.time * enemy.vibrateSpeed) * enemy.vibrateIntensity;
        return new Vector3(x, 0, z);
    }
    
}


