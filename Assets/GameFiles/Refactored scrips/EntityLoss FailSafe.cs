using UnityEngine;

public class EntityLossFailSafe : MonoBehaviour
{
    [SerializeField] private Transform teleportPos;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask enemyLayer;
    private void OnTriggerEnter(Collider other)
    {
        if ((playerLayer & (1 << other.gameObject.layer)) != 0)
        {
            Debug.LogWarning("Player Teleported via Failsafe Bound");
            other.transform.position = teleportPos.position;
        }
        else if((enemyLayer & (1 << other.gameObject.layer)) != 0)
        {
            if(other.gameObject.TryGetComponent<Entity>(out Entity entity))
            {
                Debug.LogWarning("Enemy Entity Executed by Failsafe Bound");
                entity.OnTakeDamage(99999, Color.black, DamageType.Debug);
            }
        }
        else
        {
            Debug.LogWarning("Non-Entity object collided with Failsafe Bound");
        }
    }
}
