using UnityEngine;

public interface IEnemy
{
    void OnTakeDamage(int amount);
    public void MoveTowardsPlayer();

    public void OnTakeKnockback(float knockbackForce);

    public void OnDeath();
}
