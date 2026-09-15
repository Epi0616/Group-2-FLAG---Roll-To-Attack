using UnityEngine;

public class HeavyDummyHealthSystem : DummyHealthSystem
{
    [SerializeField] private DamageType ignoreType;
    public override void OnTakeDamage(int damageAmount, DamageType type)
    {
        if (type == ignoreType) { Debug.Log("Ignored: " + type.ToString()); return; }
        currentHealth -= damageAmount;
        RunTimeStatTracker.instance.runTimeStats.totalDamageDealt += damageAmount;
        if (currentHealth <= 0)
        {
            OnDeath(type);
        }
    }
}
