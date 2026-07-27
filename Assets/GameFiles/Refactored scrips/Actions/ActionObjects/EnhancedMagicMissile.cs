using UnityEngine;

public class EnhancedMagicMissile : EnhancedSeekingRocket
{
    [SerializeField] protected TrailRenderer missileTrail;

    protected override void DestroyMe()
    {
        if (isDestroyed) return;
        isDestroyed = true;
        missileTrail.Clear();

        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
