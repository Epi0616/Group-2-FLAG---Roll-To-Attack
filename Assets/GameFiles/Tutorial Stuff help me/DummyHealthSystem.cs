using System;

public class DummyHealthSystem : EntityHealthSystem
{
    public static event Action<DamageType> DummyDeathEvent;

    public override void OnTakeDamage(int damageAmount, DamageType type)
    {
        currentHealth -= damageAmount;
        RunTimeStatTracker.instance.runTimeStats.totalDamageDealt += damageAmount;
        if (currentHealth <= 0)
        {
            OnDeath(type);
        }
    }
    public void OnDeath(DamageType type)
    {
   
        if (isDead) { return; }
        isDead = true;

        ownerEntity.statusSystem.currentActiveStatusEffects.Clear();

        DummyDeath(type);
    }

    //public void HandleDeathAfterAnimation(float delayTime)
    //{
    //    StartCoroutine(DelayedDeath(delayTime));
    //}

    //private IEnumerator DelayedDeath(float delayTime)
    //{
    //    yield return new WaitForSeconds(delayTime);
    //    DummyDeath();
    //}

    private void DummyDeath(DamageType type)
    {
        DummyDeathEvent?.Invoke(type);
        try
        {           
            ObjectPoolManager.ReturnObjectToPool(ownerEntity.gameObject, 0);
        }
        catch
        {
            Destroy(ownerEntity.gameObject);
        }
    }
}
