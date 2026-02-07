using UnityEngine;

public class SimpleRaiderEnemy : EnemyBaseClass
{
    private SimpleRaiderEnemy()
    {
        moveSpeed = 10f;
        maxHealth = 100;
        currentHealth = maxHealth;
    }
}
