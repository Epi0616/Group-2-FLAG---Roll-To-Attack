using UnityEngine;

public class EnhancedMagicMissile : EnhancedSeekingRocket
{
    [SerializeField] protected TrailRenderer missileTrail;

    protected override void DestroyMe()
    {
        if (isDestroyed) return;
        isDestroyed = true;
        
        //if (hitCount != expectedHits)
        //{
        //    Debug.LogWarning("Rocket Failed To Hit Expected Target Count");
        //    Debug.Log("Expected Hits: " + expectedHits);
        //    Debug.Log("Hits: " + hitCount);
        //}
        missileTrail.Clear();

        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
