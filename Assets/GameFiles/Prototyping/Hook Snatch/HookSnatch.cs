using UnityEngine;

public class HookSnatch : MonoBehaviour
{
    public Entity entity;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            entity.OnTakeDamage(10, Color.aliceBlue, DamageType.Normal);
        }
    }
}