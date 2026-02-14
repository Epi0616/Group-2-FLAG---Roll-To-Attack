using UnityEngine;

public class SimpleRaiderEnemy : EnemyBaseClass
{
    private SimpleRaiderEnemy()
    {
        moveSpeed = 10f;
        maxHealth = 1000;
        currentHealth = maxHealth;
    }
}
