using UnityEngine;

public class PlayerSpike : MonoBehaviour
{
    public float lifeSpan = 15f;
    public float radius = 3f;
    public float speed = 180f;
    public Vector3 desiredWorldUp;

    private float age = 0;
    private GameObject player;
    private float angle;
    private Quaternion rotation;
    private Vector3 offset;

    public void Initialize(float startAngle, GameObject player)
    {
        this.player = player;
        this.angle = startAngle;             
    }

    void Update()
    {
        CheckForExpiration();
        OrbitPlayer();
    }

    private void OrbitPlayer()
    {
        angle += speed * Time.deltaTime;

        rotation = Quaternion.Euler(0, angle, 0);
        offset = rotation * Vector3.forward * radius;
        transform.position = player.transform.position + offset;

        transform.LookAt(player.transform, desiredWorldUp);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject Enemy = collision.gameObject;
        if (Enemy.CompareTag("Enemy"))
        {
            DamageEnemy(Enemy);
        }
    }

    private void DamageEnemy(GameObject Enemy)
    {
        Enemy.GetComponent<EnemyStateController>().OnTakeDamage(4);
        DestroyMe();
    }

    private void CheckForExpiration()
    {
        age += Time.deltaTime;
        if (!(age >= lifeSpan)) { return; }

        DestroyMe();
    }

    private void DestroyMe()
    {
        player.GetComponent<PlayerStateController>().RemoveObjectFromOrbit(gameObject);
        Destroy(gameObject);
    }
}
